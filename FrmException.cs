using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ShowLib {
    //FrmException
    public partial class FE : Form {

        public FE() {
            InitializeComponent();
        }

        public FE(string title, string message) {
            InitializeComponent();
            Text = title;
            tbMessage.Text = message;

        }

        public FE(string title, string message, string details) {
            InitializeComponent();
            Text = title;
            tbMessage.Text = message;
            tbDetails.Text = details;

        }

        public static void Show(string message, string title) {
            FE f = new FE(title, message);
            f.ShowDialog();
        }

        public static void ShowNonModal(string message, string title)
        {
            FE f = new FE(title, message);
            f.Show();
        }

        public static void Show(string message, string title, string details) {
            FE f = new FE(title, message, details);
            f.ShowDialog();
        }

        public static void ShowNonModal(string message, string title, string details)
        {
            FE f = new FE(title, message, details);
            f.Show();
        }


        public static void Show(string message, string title, Exception ex)
        {
            FE f = new FE(title, message, ex.Message + " " + ex.StackTrace);
            f.ShowDialog();
        }

        public static void ShowNonModal(string message, string title, Exception ex)
        {
            FE f = new FE(title, message, ex.Message + " " + ex.StackTrace);
            f.Show();
        }


        public static void Show(Exception ex) {
            FE f = new FE("Ocorreu um erro inesperado", ex.Message, ex.StackTrace);
            f.ShowDialog();
        }

        public static void ShowNonModal(Exception ex)
        {
            FE f = new FE("Ocorreu um erro inesperado", ex.Message, ex.StackTrace);
            f.Show();
        }


        private void btClose_Click(object sender, EventArgs e) {
            Close();
        }


        private void FE_Load(object sender, EventArgs e) {

        }        
    }
}