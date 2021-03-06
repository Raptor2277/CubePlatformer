﻿using SFML.Audio;
using System.Collections.Generic;

namespace Framework.Audio
{

    class MusicPlayer
    {
        private Dictionary<string, Music> music;
        public Music currentMusic;

        public MusicPlayer()
        {
            music = new Dictionary<string, Music>();
        }

        ~MusicPlayer()
        {
            foreach (KeyValuePair<string, Music> entry in music)
                entry.Value.Dispose();
        }

        public void play()
        {
            if (currentMusic != null)
                currentMusic.Play();
        }

        public void pause()
        {
            if (currentMusic != null)
                currentMusic.Play();
        }

        public void stop()
        {
            if (currentMusic != null)
                currentMusic.Play();
        }

        public void add(string key, Music m)
        {
            music.Add(key, m);
            currentMusic = m;
        }

        public void setCurrentTrack(string key)
        {
            currentMusic = music[key];
        }
    }
}
