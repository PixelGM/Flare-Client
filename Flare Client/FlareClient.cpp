#include <iostream>
#include <vector>
#include "GuiLoader.h"
#include "LittleHacker.h"

#include "discord.h"

Discord* g_Discord;

using namespace std;

int main()
{

	g_Discord->Initialize();
	g_Discord->Update((char*)"On the main menu", 0, -1);
	DWORD procID;
	uintptr_t ModuleBase;
	HANDLE hProcess;

	procID = mem::GetProcId(L"Minecraft.Windows.exe");
	if (procID == NULL) {
		cout << "Unable to locate process, make sure Minecraft is running!\n";
	}
	else
	{
		cout << "Minecraft Process ID: " << procID << "\n";
		hProcess = OpenProcess(PROCESS_ALL_ACCESS, NULL, procID);
		ModuleBase = mem::GetModuleBaseAddress(procID, L"Minecraft.Window.exe");

		//Branding patch (IDK where else to put it)
		uintptr_t ccpb = mem::moduleBase + 0x1EA8A2E;
		uintptr_t eppb = mem::moduleBase + 0x1967EE;
		BYTE ccb[120] = { 0xEB, 0x05, 0xE9, 0xBE, 0xDD, 0x2E, 0xFE, 0x0F, 0x2F, 0x05, 0x3B, 0x00, 0x00, 0x00, 0x75, 0x2B, 0xF3, 0x0F, 0x7E, 0x05, 0x3D, 0x00, 0x00, 0x00, 0x0F, 0x11, 0x06, 0xF3, 0x0F, 0x7E, 0x05, 0x3A, 0x00, 0x00, 0x00, 0x48, 0x83, 0xC6, 0x08, 0x0F, 0x11, 0x06, 0xF3, 0x0F, 0x7E, 0x05, 0x33, 0x00, 0x00, 0x00, 0x48, 0x83, 0xC6, 0x08, 0x0F, 0x11, 0x06, 0xEB, 0x03, 0x0F, 0x11, 0x06, 0x0F, 0x10, 0x4F, 0x10, 0xEB, 0xBE, 0x90, 0x00, 0x00, 0x00, 0x00, 0x76, 0x31, 0x2E, 0x31, 0x34, 0x2E, 0x31, 0x00, 0x00, 0x0F, 0x00, 0x00, 0xC2, 0xA7, 0x61, 0x46, 0x6C, 0x61, 0x72, 0x65, 0x2D, 0x31, 0x2E, 0x31, 0x34, 0x2E, 0x31, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
		mem::PatchEx((BYTE*)ccpb, ccb, 120, hProcess);
		BYTE epb[7] = { 0xE9, 0x3B, 0x22, 0xD1, 0x01, 0x90, 0x90 };
		mem::PatchEx((BYTE*)eppb, epb, 7, hProcess);

		GuiLoader f;
	}
}