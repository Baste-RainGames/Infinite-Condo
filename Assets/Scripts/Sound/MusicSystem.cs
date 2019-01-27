using System.Collections.Generic;
using UnityEngine;

public static class Songs
{
    public const string MainTheme = "MainTheme";
    public const string Victory = "Victory";
    public const string GameOver = "GameOver";
    public static Dictionary<string, Song> SongDictionary = new Dictionary<string, Song>()
    {
        { MainTheme, new Song(MainTheme, "event:/Soundtracks/" + MainTheme, new List<string>(){"ToMain", "ToIntense"}) },
        { Victory, new Song(Victory, "event:/Soundtracks/" + Victory) },
        { GameOver, new Song(GameOver, "event:/Soundtracks/" + GameOver) }
    };
}

public static class SoundEffects
{
    public static Dictionary<string, string> SoundEffectDictionary = new Dictionary<string, string>()
    {
        { "Alarm", "event:/Soundeffects/Alarm" },
        { "Bedroom", "event:/Soundeffects/Bedroom" },
        { "Bookpaper", "event:/Soundeffects/Bookpaper" },
        { "CatActivate", "event:/Soundeffects/CatActivate" },
        { "CatSad", "event:/Soundeffects/CatSad" },
        { "Coins1", "event:/Soundeffects/Coins1" },
        { "Coins2", "event:/Soundeffects/Coins2" },
        { "Condo", "event:/Soundeffects/Condo" },
        { "Condobreaker", "event:/Soundeffects/Condobreaker" },
        { "CoolguyActive", "event:/Soundeffects/CoolguyActivate" },
        { "CoolguySad", "event:/Soundeffects/CoolguySad" },
        { "Death1", "event:/Soundeffects/Death1" },
        { "Death2", "event:/Soundeffects/Death2" },
        { "DogActivate", "event:/Soundeffects/DogActivate" },
        { "DogSad", "event:/Soundeffects/DogSad" },
        { "Door", "event:/Soundeffects/Door" },
        { "DropRoom", "event:/Soundeffects/DropRoom" },
        { "Explosion", "event:/Soundeffects/Explosion" },
        { "FallingBox", "event:/Soundeffects/FallingBox" },
        { "Gym", "event:/Soundeffects/Gym" },
        { "ManActivate", "event:/Soundeffects/ManActivate" },
        { "ManSad", "event:/Soundeffects/ManSad" },
        { "Menu", "event:/Soundeffects/Menu" },
        { "Menu2", "event:/Soundeffects/Menu2" },
        { "Points", "event:/Soundeffects/Points" },
        { "Rotate", "event:/Soundeffects/Rotate" },
        { "Rotate2", "event:/Soundeffects/Rotate2" },
        { "SharkAttack", "event:/Soundeffects/SharkAttack" },
        { "Stairs", "event:/Soundeffects/Stairs" },
        { "Toilet", "event:/Soundeffects/Toilet" },
        { "WomanActivate", "event:/Soundeffects/WomanActivate" },
        { "WomanSad", "event:/Soundeffects/WomanSad" },
    };
}

public class Song
{
	public string Name;
	public List<string> Parameters;
	public string EventLocation;

	public Song(string name, string eventLocation, List<string> parameters)
	{
		Parameters = new List<string>();
		EventLocation = eventLocation;
		Parameters.AddRange(parameters);
	}

	public Song(string name, string eventLocation)
	{
		Parameters = new List<string>();
		EventLocation = eventLocation;
	}
}

public static class MusicSystem
{

    private static FMOD.Studio.EventInstance musicInstance;
    private static Song currentSong;

	public static void PlaySong(Song song)
	{
        if (currentSong.Name.Equals(song.Name))
        {
            return;
        }
        StopSong();
		musicInstance = FMODUnity.RuntimeManager.CreateInstance(song.EventLocation);
		musicInstance.setVolume(Tweaks.Instance.musicVolume);
		musicInstance.start();
	}

	public static void PauseCurrentSong()
	{
		musicInstance.setPaused(true);
	}

	public static void ResumeCurrentSong()
	{
		musicInstance.setPaused(false);
	}

	public static void StopSong()
	{
		musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		musicInstance.release();
	}

	public static void PlaySoundEffect(string path)
	{
		var soundEffect = FMODUnity.RuntimeManager.CreateInstance(path);
		soundEffect.setVolume(Tweaks.Instance.sfxVolume);
		soundEffect.start();
	}

    public static void PlaySongPart(string part)
    {
        if (currentSong.Parameters.Exists(text => text == part))
        {
            List<string> setToFalse = currentSong.Parameters.FindAll(text => text != part);
            foreach (var parameter in setToFalse)
            {
                musicInstance.setParameterValue(parameter, 0f);
            }
            FMOD.RESULT result = musicInstance.setParameterValue(part, 1f);
            if (result != FMOD.RESULT.OK)
            {
                Debug.Log("Something went wrong when setting the parameter");
            }
        }
        else
        {
            Debug.Log("Invalid argument");
        }
    }
}
