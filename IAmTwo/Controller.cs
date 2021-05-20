using System;
using System.Collections.Generic;
using OpenTK.Input;
using SM.Utils.Controls;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo
{
    public static class Controller
    {
        private static Dictionary<string, Tuple<string, string>> _controllerCorrectNames = new Dictionary<string, Tuple<string, string>>()
        {
            {"A", new Tuple<string, string>("a", "x")},
        };

        private static GameKeybindActor _actor;

        static GameKeybindHost _keybindHost = new GameKeybindHost(new GameKeybindList()
        {
            {"p_move", context => (float)(context.KeyboardState[Key.A] ? -1 : 0) + (context.KeyboardState[Key.D] ? 1 : 0), context => context.ControllerState?.Thumbs.Left.X },
            {"p_jump", context => context.KeyboardState[Key.Space], context => context.ControllerState?.Buttons.A},
            {"l_start", context => Keyboard.IsDown(Key.Space, true), context => context.ControllerState?.Buttons.A }
        });

        public static GameKeybindActor Actor
        {
            get => _actor;
            set
            {
                _actor = value;
                _actor.ConnectHost(_keybindHost);
            }
        }

        public static bool IsController => _actor.Type == GameKeybindActorType.Controller;

        public static bool IsPS = false;

        public static string GetCorrectName(string button)
        {
            return IsPS ? _controllerCorrectNames[button].Item2 : _controllerCorrectNames[button].Item1;
        }
    }
}