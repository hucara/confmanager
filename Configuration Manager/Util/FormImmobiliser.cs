using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Configuration_Manager.Util
{
    public sealed class FormImmobiliser : NativeWindow
    {
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(Rectangle r)
            {
                this.Left = r.Left;
                this.Top = r.Top;
                this.Right = r.Right;
                this.Bottom = r.Bottom;
            }
        }

        private const int WM_MOVING = 0x216;
        private const int TRUE = 1;

        private Form target;

        public FormImmobiliser(Form target)
        {
            this.target = target;
            this.target.HandleCreated += new EventHandler(target_HandleCreated);
            this.target.HandleDestroyed += new EventHandler(target_HandleDestroyed);
        }

        void target_HandleCreated(object sender, EventArgs e)
        {
            // Listent to the target forms message queue.
            this.AssignHandle(this.target.Handle);
        }

        void target_HandleDestroyed(object sender, EventArgs e)
        {
            this.ReleaseHandle();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOVING)
            {
                // Reset the new location to the same as the current location.
                Marshal.StructureToPtr(new RECT(this.target.Bounds),
                                       m.LParam,
                                       false);
                m.Result = new IntPtr(TRUE);
            }

            base.WndProc(ref m);
        }
    }
}
