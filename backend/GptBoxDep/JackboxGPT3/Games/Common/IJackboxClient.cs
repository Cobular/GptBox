using System;
using JackboxGPT3.Games.Common.Models;
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace JackboxGPT3.Games.Common
{
    public interface IJackboxClient
    {
        public void Connect(string player_name, string room_code);
        public GameStatus GetGameStatus();
        public event EventHandler OnDisconnect;
    }
}