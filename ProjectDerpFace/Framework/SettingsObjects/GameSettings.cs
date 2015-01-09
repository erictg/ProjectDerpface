using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace ProjectDerpface.Framework.SettingsObjects
{
    public class GameSettings
    {
        //display mode
        public int displayMode { get; set; }
        public const int FULL_SCREEN = 0;
        public const int WINDOW = 1;

        //render distance
        public int renderDistanceLevel { get; set; }
        public const int LOW = 0;
        public const int MEDIUM = 1;
        public const int HIGH = 2;
        public const int ULTRA_HIGH = 3;

        //brightness
        public int brightness { get; set; }
        public const int BRIGHTNESS_MIN = 0;
        public const int BRIGHTNESS_MAX = 100;

        //resolution
        public int resolution { get; set; }
        //come up with resolutions later

        //sound effect volume
        public int soundEffectVolume { get; set; }
        public const int SOUND_EFFECT_VOLUME_MIN = 0;
        public const int SOUNT_EFFECT_VOLUME_MAX = 100;

        //music
        public int musicVolume { get; set; }
        public const int MUSIC_VOLUME_MIN = 0;
        public const int MUSIC_VOLUME_MAX = 100;

        public Controls controls;
        //sound source
        public int maxSoundSources
        {
            get
            {
                return maxSoundSources;
            }

            set
            {
                if (value < MIN_SOUND_SOURCES)
                {
                    value = MIN_SOUND_SOURCES;
                }
                else if (value > MAX_SOUND_SOURCES)
                {
                    value = MAX_SOUND_SOURCES;
                }
                else
                {
                    maxSoundSources = value;
                }
            }
        }
        public const int MIN_SOUND_SOURCES = 50;
        public const int MAX_SOUND_SOURCES = 150;

        //entities on screen
        public int maxEntitiesOnScreen 
        {
            get
            {
                return maxEntitiesOnScreen;
            }

            set
            {
                if (value < MIN_ENTITES)
                {
                    value = MIN_ENTITES;
                }
                else if(value > MAX_ENTITES)
                {
                    value = MAX_ENTITES;
                }
                else
                {
                    maxEntitiesOnScreen = value;
                }
            }
 
        }
        public const int MIN_ENTITES = 15;
        public const int MAX_ENTITES = 50;

        //explicit speach 
        public bool explicitSpeach { get; set; }

        public GameSettings()
        {
            displayMode = FULL_SCREEN;
            renderDistanceLevel = HIGH;
            brightness = 50;
            //resolution preset here
            soundEffectVolume = 45;
            musicVolume = 45;
           // maxEntitiesOnScreen = 25;
            explicitSpeach = true;
            //XmlControls.deserializeObject<Controls>("defaultControls", ref controls);
            controls = new Controls();
            System.Diagnostics.Debug.WriteLine("saving");
            XmlControls.serializeObject<Controls>(XmlControls.SETTINGS_KEYS, "defaultControls", controls);
        }
    }

    public class AudioSettings
    {
        public int effectsLevel { get; set; }
        public int musicLevel { get; set; }
        public int maxSoundSources { get; set; }


        public AudioSettings(int effectsLevel, int musicLevel, int maxSoundSources)
        {
            this.effectsLevel = effectsLevel;
            this.musicLevel = musicLevel;
            this.maxSoundSources = maxSoundSources;
        }
        public AudioSettings()
        {
            effectsLevel = 50;
            musicLevel = 50;
            maxSoundSources = 75;
        }
    }
    public struct ScreenResolution
    {
        public int x;
        public int y;

        public ScreenResolution(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void updateScreenResolution(Game game)
        {
            (game as Game1).Graphics.PreferredBackBufferHeight = y;
            (game as Game1).Graphics.PreferredBackBufferWidth = x;
            (game as Game1).Graphics.ApplyChanges();
        }
    }
    public class VideoSettings
    {
        public int maxFPS { get; set; }
        public enum resolution {r1366x768, r1920x1080, r1440x900, r1280x1024, r1600x900, r1024x768, r1680x1050, r1920x1200, r1360x768, r1280x720 };
        public resolution currentResolution { get; set; }
        public ScreenResolution screenResolution;
        public float brightnessLevel { get; set; } //max 100
        //default constructor
        public VideoSettings()
        {
            maxFPS = 60;
            currentResolution = resolution.r1366x768;
            screenResolution = determineResolution(currentResolution);
            brightnessLevel = 75;
        }
        //other constructor
        public VideoSettings(int maxFPS, resolution currentResolution, float brightnessLevel)
        {
            this.maxFPS = maxFPS;
            this.currentResolution = currentResolution;
            screenResolution = determineResolution(currentResolution);
            this.brightnessLevel = brightnessLevel;
        }

        public ScreenResolution determineResolution(resolution res)
        {
            switch (res)
            {
                case resolution.r1024x768:
                    return new ScreenResolution(1024, 768);
                case resolution.r1280x1024:
                    return new ScreenResolution(1280, 1024);
                case resolution.r1280x720:
                    return new ScreenResolution(1280, 720);
                case resolution.r1360x768:
                    return new ScreenResolution(1360, 768);
                case resolution.r1366x768:
                    return new ScreenResolution(1366, 768);
                case resolution.r1440x900:
                    return new ScreenResolution(1440, 900);
                case resolution.r1600x900:
                    return new ScreenResolution(1600, 900);
                case resolution.r1680x1050:
                    return new ScreenResolution(1680, 1050);
                case resolution.r1920x1080:
                    return new ScreenResolution(1920, 1080);
                case resolution.r1920x1200:
                    return new ScreenResolution(1920, 1200);  
            }
            return new ScreenResolution();
        }
    }
}
