using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Philosophers
{
    public class Philosopher
    {
        private Label output;
        private Label fork1;
        private Label fork2;
        //private bool status; // thinking (true), eating (false)
        public Thread thrd;
        private Mutex mtxL;
        private Mutex mtxR;
        private int count; // Amount of times have started eating

        public Philosopher(Label label, Label leftFork, Label rightFork, Mutex mutL, Mutex mutR, int x = 1)
        {
            // Assign labels
            output = label;
            fork1 = leftFork;
            fork2 = rightFork;
            // Assign mutexes
            mtxL = mutL;
            mtxR = mutR;
            // Start count
            count = 0;
            // Start thread
            thrd = new Thread(this.Run);
            if(x == 1)
                thrd.Priority = ThreadPriority.AboveNormal;
            thrd.Start();
        }

        // Delegates for lables
        private delegate void DisplayDelegate(string text, System.Drawing.Color color);
        private delegate void EnableDisableDelegate1();
        private delegate void EnableDisableDelegate2();

        // Display text on label
        private void DisplayText(string text, System.Drawing.Color color)
        {
            output.Text = text + "\n" + count;
            output.BackColor = color;
        }
        // Enable/disable fork to the left
        private void EnableDisableFork1()
        {
            fork1.Enabled = !fork1.Enabled;
        }
        // Enable/disable fork to the right
        private void EnableDisableFork2()
        {
            fork2.Enabled = !fork2.Enabled;
        }

        public void Run()
        {
            while (true)
            {
                // If not eating, we're thinking
                output.Invoke(new DisplayDelegate(DisplayText), new object[] { "Thinking", System.Drawing.Color.LightPink });

                // Attempt to grab two forks
                try
                {
                    if (mtxR.WaitOne() && mtxL.WaitOne())
                    {
                        // Disable the forks
                        fork1.Invoke(new EnableDisableDelegate1(EnableDisableFork1));
                        fork2.Invoke(new EnableDisableDelegate2(EnableDisableFork2));
                        // Increment count
                        count++;
                        // Eat for 3 seconds
                        output.Invoke(new DisplayDelegate(DisplayText), new object[] { "Eating", System.Drawing.Color.LightGreen });
                        Thread.Sleep(3000);
                        // Release Forks
                        fork1.Invoke(new EnableDisableDelegate1(EnableDisableFork1));
                        fork2.Invoke(new EnableDisableDelegate2(EnableDisableFork2));
                        mtxR.ReleaseMutex();
                        mtxL.ReleaseMutex();
                    }
                }
                catch
                { 
                    // Do nothing if abandoned mutex
                }
            } // End while
        } // End Run()
    } // end class
} // end namespace
