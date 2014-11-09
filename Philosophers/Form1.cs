using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Philosophers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Mutexes (synced with the fork labels)
        public static Mutex mtx1 = new Mutex();
        public static Mutex mtx2 = new Mutex();
        public static Mutex mtx3 = new Mutex();
        public static Mutex mtx4 = new Mutex();
        public static Mutex mtx5 = new Mutex();
        // Philosopher classes
        private Philosopher philosOne;
        private Philosopher philosTwo;
        private Philosopher philosThree;
        private Philosopher philosFour;
        private Philosopher philosFive;

        private void Form1_Load_1(object sender, EventArgs e)
        {
            lblPhilos1Status.Text = "Test1";
            // Create philosophers
            philosFive = new Philosopher(lblPhilos5Status, lblFork3, lblFork4, mtx3, mtx4, 1);
            philosOne = new Philosopher(lblPhilos1Status,lblFork1,lblFork5,mtx1,mtx5);
            philosTwo = new Philosopher(lblPhilos2Status,lblFork1,lblFork2,mtx1,mtx2);
            philosThree = new Philosopher(lblPhilos3Status, lblFork5, lblFork4, mtx5, mtx4);
            philosFour = new Philosopher(lblPhilos4Status, lblFork2, lblFork3, mtx2, mtx3);
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Close all threads
            philosOne.thrd.Abort();
            philosTwo.thrd.Abort();
            philosThree.thrd.Abort();
            philosFour.thrd.Abort();
            philosFive.thrd.Abort();
        }
    }
}
