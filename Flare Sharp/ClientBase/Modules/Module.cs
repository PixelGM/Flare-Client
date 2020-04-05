﻿using Flare_Sharp.ClientBase.Categories;
using Flare_Sharp.ClientBase.IO;
using Flare_Sharp.ClientBase.Modules.Settings;
using Flare_Sharp.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Flare_Sharp.ClientBase.Modules
{
    public abstract class Module
    {
        public string name;
        public bool enabled;
        public bool selected;
        public int keybind;

        private bool wasEnabled = false;
        public EventHandler toggleEvent;

        public Module(string name, Category category, int keybind, bool enabled)
        {
            this.name = name;
            this.keybind = keybind;
            this.enabled = enabled;
            category.modules.Add(this);
            bool succ = false;
            bool enabl = ProfileIO.loadSetting<bool>(name + ".enabled", out succ);
            if (succ)
            {
                enabled = enabl;
            }
        }

        public List<SliderSetting> sliderSettings = new List<SliderSetting>();
        public void RegisterSliderSetting(string text, int min, int value, int max)
        {
            sliderSettings.Add(new SliderSetting(text, min, value, max));
        }
        public List<ToggleSetting> toggleSettings = new List<ToggleSetting>();
        public void RegisterToggleSetting(string text, bool value)
        {
            toggleSettings.Add(new ToggleSetting(text, value));
        }

        public void startTimer(int millis)
        {
            Timer timer = new Timer();
            timer.Interval = millis;
            timer.Elapsed += (object send, ElapsedEventArgs arg) =>
            {
                if (enabled)
                {
                    onTimedTick();
                }
            };
            timer.Start();
        }


        public virtual void onEnable()
        {
            this.enabled = true;
            //
        }
        public virtual void onDisable()
        {
            this.enabled = false;
            //
        }
        //Called like a loop when enabled
        public virtual void onTick()
        {
            
        }
        //Called no matter what
        public virtual async Task onLoop()
        {
            if (wasEnabled != enabled)
            {
                if (enabled == false)
                {
                    onDisable();
                    try
                    {
                        ProfileIO.saveSetting<bool>(name + ".enabled", false);
                        if (toggleEvent != null)
                            toggleEvent.Invoke(this, new EventArgs());
                    }
                    catch (Exception) { }
                }
                else
                {
                    onEnable();
                    try
                    {
                        ProfileIO.saveSetting<bool>(name + ".enabled", true);
                        if (toggleEvent!=null)
                            toggleEvent.Invoke(this, new EventArgs());
                    }
                    catch (Exception) { }
                }
                wasEnabled = enabled;
            }
            if (enabled)
            {
                try
                {
                    onTick();
                }
                catch (Exception) { }
            }
            return;
        }

        //Called at a defined time repeatedly
        public virtual void onTimedTick()
        {

        }
    }
}
