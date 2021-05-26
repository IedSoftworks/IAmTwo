using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using IAmTwo.LevelObjects;
using OpenTK;
using OpenTK.Input;
using SharpDX.XInput;
using SM.Base;
using SM.Base.Types;
using SM.Utils.Controls;
using SM2D.Controls;
using Keyboard = SM.Base.Controls.Keyboard;
using Mouse = SM.Base.Controls.Mouse;
using IAmTwo.Game;

namespace IAmTwo
{
    public static class Controller
    {

        private static Dictionary<string, Tuple<string, string>> _controllerCorrectNames = new Dictionary<string, Tuple<string, string>>()
        {
            {"A", new Tuple<string, string>("a", "x")},
            {"Y", new Tuple<string, string>("y", "t")},
            {"B", new Tuple<string, string>("b", "c")},
            {"Icon", new Tuple<string, string>("g", "h")}
        };

        private static GameKeybindActor _actor;

        static GameKeybindHost _keybindHost = new GameKeybindHost(new GameKeybindList()
        {
            {"p_move", context => (float)(context.KeyboardState[Key.A] ? -1 : 0) + (context.KeyboardState[Key.D] ? 1 : 0), context => context.ControllerState.Thumbs.Left.X },
            {"p_jump", context => context.KeyboardState[Key.Space], context => context.ControllerState.Buttons.A},

            {"l_start", context => Keyboard.IsDown(Key.Space, true), context => context.ControllerState.Buttons[GamepadButtonFlags.A, true] },
            {"l_continue", context => Keyboard.IsDown(Key.Space, true), context => context.ControllerState.Buttons[GamepadButtonFlags.A, true]},
            {"l_retry", context => Keyboard.IsDown(Key.R, true), context => context.ControllerState.Buttons[GamepadButtonFlags.Y, true]},
            {"l_exit", context => Keyboard.IsDown(Key.Escape, true), context => context.ControllerState.Buttons[GamepadButtonFlags.B, true]},

            {"g_click", context => Mouse.LeftClick, context => 
                context.ControllerState.Buttons[GamepadButtonFlags.A, true] }
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
        public static GameController ControllerHandle = new GameController(0);

        public static bool IsController => _actor.Type == GameKeybindActorType.Controller;

        public static bool IsPS => UserSettings.PlaystationLayout;

        public static bool AllowedCursor => Mouse.StopTracking && !(SMRenderer.CurrentWindow.CurrentScene is PlayScene || SMRenderer.CurrentWindow.CurrentScene is CreditsScene);

        public static string GetCorrectName(string button)
        {
            return IsPS ? _controllerCorrectNames[button].Item2 : _controllerCorrectNames[button].Item1;
        }
        
        public static void MouseCursor(object sender, FrameEventArgs args)
        {
            if (AllowedCursor)
            {
                Mouse.InScreen += ControllerHandle.GetState().Thumbs.Left * new Vector2(1, -1) * 5;
            }
        }
    }
}