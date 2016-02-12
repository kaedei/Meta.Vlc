﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace xZune.Vlc
{
    /// <summary>
    /// A warpper for <see cref="Interop.MediaPlayer.AudioOutput"/> struct.
    /// </summary>
    public class AudioOutput
    {
        internal AudioOutput(IntPtr pointer)
        {
            _pointer = pointer;
            if (pointer != IntPtr.Zero)
            {
                Marshal.PtrToStructure(pointer, _struct);
                Name = InteropHelper.PtrToString(_struct.Name);
                Description = InteropHelper.PtrToString(_struct.Description);
            }
        }

        internal Interop.MediaPlayer.AudioOutput _struct;
        internal IntPtr _pointer;
        
        public String Name { get; private set; }
        
        public String Description { get; private set; }
    }

    /// <summary>
    /// A list warpper for <see cref="Interop.MediaPlayer.AudioOutput"/> linklist struct.
    /// </summary>
    public class AudioOutputList : IDisposable, IEnumerable<AudioOutput>, IEnumerable
    {
        /// <summary>
        /// Create a readonly list by a pointer of <see cref="Interop.MediaPlayer.AudioOutput"/>.
        /// </summary>
        /// <param name="pointer"></param>
        public AudioOutputList(IntPtr pointer)
        {
            _list = new List<AudioOutput>();
            _pointer = pointer;

            while (pointer != IntPtr.Zero)
            {
                var audioOutput = new AudioOutput(pointer);
                _list.Add(audioOutput);

                pointer = audioOutput._struct.Next;
            }
        }

        private List<AudioOutput> _list;
        private IntPtr _pointer;

        public IEnumerator<AudioOutput> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public AudioOutput this[int index]
        {
            get { return _list[index]; }
        }

        public void Dispose()
        {
            if (_pointer == IntPtr.Zero) return;

            VlcMediaPlayer.ReleaseAudioOutputList(_pointer);
            _pointer = IntPtr.Zero;
            _list.Clear();
        }
    }
}