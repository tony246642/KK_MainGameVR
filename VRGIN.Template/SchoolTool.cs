﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Valve.VR;
using VRGIN.Controls;
using VRGIN.Controls.Tools;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Modes;
using VRGIN.U46.Visuals;
using VRGIN.Visuals;
using static SteamVR_Controller;
using VRGIN.Native;
using static VRGIN.Native.WindowsInterop;
using WindowsInput.Native;

namespace KoikatuVR
{
    public class SchoolTool : Tool
    {
        public override Texture2D Image
        {
            get
            {
                return UnityHelper.LoadImage("icon_school.png");
            }
        }

        private void cam2pl()
        {
            var player = GameObject.Find("ActionScene/Player").transform;
            var playerHead = GameObject.Find("ActionScene/Player/chaM_001/BodyTop/p_cf_body_bone_low/cf_j_root/cf_n_height/cf_j_hips/cf_j_spine01/cf_j_spine02/cf_j_spine03/cf_j_neck/cf_j_head/cf_s_head").transform;
            var cam = GameObject.Find("VRGIN_Camera (origin)").transform;
            var headCam = GameObject.Find("VRGIN_Camera (origin)/VRGIN_Camera (eye)/VRGIN_Camera (head)").transform;

            var pos = playerHead.position;
            pos += cam.position - headCam.position;
            //pos.y += 1.85f;

            cam.rotation = player.rotation;
            var delta_y =  cam.rotation.eulerAngles.y - headCam.rotation.eulerAngles.y;
            cam.Rotate(Vector3.up * delta_y);
            Vector3 cf = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            cam.position = pos - cf * 0.1f;
        }

        private void pl2cam(Boolean onlyRotation = false)
        {
            var player = GameObject.Find("ActionScene/Player").transform;
            var playerHead = GameObject.Find("ActionScene/Player/chaM_001/BodyTop/p_cf_body_bone_low/cf_j_root/cf_n_height/cf_j_hips/cf_j_spine01/cf_j_spine02/cf_j_spine03/cf_j_neck/cf_j_head/cf_s_head").transform;
            //var cam = GameObject.Find("VRGIN_Camera (origin)").transform;
            var headCam = GameObject.Find("VRGIN_Camera (origin)/VRGIN_Camera (eye)/VRGIN_Camera (head)").transform;

            var pos = headCam.position;
            pos.y += player.position.y - playerHead.position.y;

            var delta_y = headCam.rotation.eulerAngles.y - player.rotation.eulerAngles.y;
            player.Rotate(Vector3.up * delta_y);
            Vector3 cf = Vector3.Scale(player.forward, new Vector3(1, 0, 1)).normalized;

            if (!onlyRotation)
            {
                player.position = pos - cf * 0.1f;
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnStart()
        {
            base.OnStart();

        }

        protected override void OnDestroy()
        {
            // nothing to do.
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            var device = this.Controller;

            if (device.GetPressDown(ButtonMask.Trigger))
            {
                pl2cam(true);
                VR.Input.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
                VR.Input.Mouse.LeftButtonDown();
            }

            if (device.GetPressUp(ButtonMask.Trigger))
            {
                VR.Input.Mouse.LeftButtonUp();
                VR.Input.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
            }

            if (device.GetPress(ButtonMask.Trigger))
            {
                cam2pl();
            }


            if (device.GetPress(ButtonMask.Grip))
            {
                pl2cam();
            }

            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Vector2 touchPosition = device.GetAxis();
                if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
                {
                    if (touchPosition.y > 0) // up
                    {
                        VR.Input.Keyboard.KeyDown(VirtualKeyCode.F3);
                    }
                    else // down
                    {
                        VR.Input.Keyboard.KeyDown(VirtualKeyCode.F4);
                    }
                }
                else
                {
                    if (touchPosition.x > 0) // right
                    {
                        VR.Input.Keyboard.KeyDown(VirtualKeyCode.F2);
                    }
                    else // left
                    {
                        VR.Input.Keyboard.KeyDown(VirtualKeyCode.F5);
                    }
                }
            }

            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Vector2 touchPosition = device.GetAxis();
                if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
                {
                    if (touchPosition.y > 0) // up
                    {
                        VR.Input.Keyboard.KeyUp(VirtualKeyCode.F3);
                    }
                    else // down
                    {
                        VR.Input.Keyboard.KeyUp(VirtualKeyCode.F4);
                    }
                }
                else
                {
                    if (touchPosition.x > 0) // right
                    {
                        VR.Input.Keyboard.KeyUp(VirtualKeyCode.F2);
                    }
                    else // left
                    {
                        VR.Input.Keyboard.KeyUp(VirtualKeyCode.F5);
                    }
                }
            }
        }

        public override List<HelpText> GetHelpTexts()
        {
            return new List<HelpText>(new HelpText[] {
                HelpText.Create("Tap to click", FindAttachPosition("trackpad"), new Vector3(0, 0.02f, 0.05f)),
                HelpText.Create("Slide to move cursor", FindAttachPosition("trackpad"), new Vector3(0.05f, 0.02f, 0), new Vector3(0.015f, 0, 0)),
                HelpText.Create("Attach/Remove menu", FindAttachPosition("lgrip"), new Vector3(-0.06f, 0.0f, -0.05f))

            });
        }
    }
}