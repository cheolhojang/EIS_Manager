﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;

namespace EIS_Manager
{
    public partial class Fitter : Form
    {
        public List<Double> freq = new List<double>();
        public List<Double> re = new List<double>();
        public List<Double> im = new List<double>();
        public List<Double> fit_re = new List<double>();
        public List<Double> fit_im = new List<double>();
        public bool fitted = new bool();
        public string python_script_location;
        public string[] pre_test;
        public string[] post_test;
        public string temp;

        //public string[] folder_files = new string[]();
        public Fitter()
        {
            InitializeComponent();
            nvyquist.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            nvyquist.ChartAreas[0].CursorX.IsUserEnabled = true;
            nvyquist.ChartAreas[0].CursorX.LineColor = Color.Transparent;
            nvyquist.ChartAreas[0].CursorX.SelectionColor = Color.Lime;
            nvyquist.ChartAreas[0].CursorX.Interval = 0;
            nvyquist.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            nvyquist.ChartAreas[0].AxisX2.ScaleView.Zoomable = true;

            nvyquist.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            nvyquist.ChartAreas[0].CursorY.IsUserEnabled = true;
            nvyquist.ChartAreas[0].CursorY.LineColor = Color.Transparent;
            nvyquist.ChartAreas[0].CursorY.SelectionColor = Color.Lime;
            nvyquist.ChartAreas[0].CursorY.Interval = 0;
            nvyquist.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            nvyquist.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
        }
        private void python_scripts_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                python_script_location = folderBrowserDialog2.SelectedPath.ToString();
                Debug.WriteLine(python_script_location);
            }
        }

        private string[] path_listing(string path)
        {
            
            string pt1 = python_script_location;
            string progToRun = pt1 + "\\path_listing.py";
            char[] splitter = { '\r' };

            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            // call hello.py to concatenate passed parameters

            //"from tools import *; print(mpt_data(path=r'C:\Users\cjang\Desktop\Kyler_Speed_Circuit\data\\',data = ['DE_49_8_30.mpt']).guess_and_plot(mask = [1000018.6, 28]))"
            proc.StartInfo.Arguments = string.Concat(progToRun, " ", path.ToString());
            proc.Start();

            StreamReader sReader = proc.StandardOutput;
            string[] output = sReader.ReadToEnd().Split(splitter);

            //foreach (string s in output)
            //Console.WriteLine(s);

            return output;
        }

        private string[] mpt_dataframe(string raw_path, string mpt_file)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            string pt1 = python_script_location;
            string progToRun = pt1 + "\\mpt_dataframe.py";
            char[] splitter = { '\r' };
            //"from tools import *; print(mpt_data(path=r'C:\Users\cjang\Desktop\Kyler_Speed_Circuit\data\\',data = ['DE_49_8_30.mpt']).guess_and_plot(mask = [1000018.6, 28]))"
            proc.StartInfo.Arguments = progToRun + " " + raw_path + " " + mpt_file;
            proc.Start();

            StreamReader sReader = proc.StandardOutput;
            string[] output = sReader.ReadToEnd().Split(splitter);

            return output;
        }

        private string[] guesser(string raw_path, string mpt_file)
        {
            string pt1 = python_script_location;
            string progToRun = pt1 + "\\guesser.py";
            char[] splitter = { '\r' };

            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            // call hello.py to concatenate passed parameters

            //"from tools import *; print(mpt_data(path=r'C:\Users\cjang\Desktop\Kyler_Speed_Circuit\data\\',data = ['DE_49_8_30.mpt']).guess_and_plot(mask = [1000018.6, 28]))"
            
            proc.StartInfo.Arguments = progToRun + " " + raw_path + " " + mpt_file + " ";
           
            //Console.WriteLine("Processing...");
            
            proc.Start();
            MessageBox.Show("Python is Processing; Error Box will fill when process is finished");
            StreamReader sReader = proc.StandardOutput;
            string[] output = sReader.ReadToEnd().Split(splitter);
            
            //foreach (string s in output)
            //Console.WriteLine(s);
            return output;
        }

        

        private string[] masker(string path, string mpt_file, string mask_choice)
        {
            string pt1 = python_script_location;
            string progToRun = pt1 + "\\masker.py";
            char[] splitter = { '\r' };

            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            // call hello.py to concatenate passed parameters

            //"from tools import *; print(mpt_data(path=r'C:\Users\cjang\Desktop\Kyler_Speed_Circuit\data\\',data = ['DE_49_8_30.mpt']).guess_and_plot(mask = [1000018.6, 28]))"
            proc.StartInfo.Arguments = string.Concat(progToRun, " ", path, " ", mpt_file, " ", mask_choice);
            //Console.WriteLine("Processing...");
            proc.Start();

            StreamReader sReader = proc.StandardOutput;
            string[] output = sReader.ReadToEnd().Split(splitter);

            return output;
        }

        private string[] masked_mpt(string path, string mpt_file, string mask_choice)
        {
            string pt1 = python_script_location;
            string progToRun = pt1 + "\\masked_df.py";
            char[] splitter = { '\r' };

            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            // call hello.py to concatenate passed parameters

            //"from tools import *; print(mpt_data(path=r'C:\Users\cjang\Desktop\Kyler_Speed_Circuit\data\\',data = ['DE_49_8_30.mpt']).guess_and_plot(mask = [1000018.6, 28]))"
            proc.StartInfo.Arguments = string.Concat(progToRun, " ", path, " ", mpt_file, " ", mask_choice);
            //Console.WriteLine("Processing...");
            proc.Start();

            StreamReader sReader = proc.StandardOutput;
            string[] output = sReader.ReadToEnd().Split(splitter);

            return output;
        }

        private string[] masked_guesser(string path, string mpt_file, string mask_choice)
        {
            string pt1 = python_script_location;
            string progToRun = pt1 + "\\masked_guesser.py";
            char[] splitter = { '\r' };

            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            // call hello.py to concatenate passed parameters

            //"from tools import *; print(mpt_data(path=r'C:\Users\cjang\Desktop\Kyler_Speed_Circuit\data\\',data = ['DE_49_8_30.mpt']).guess_and_plot(mask = [1000018.6, 28]))"
            proc.StartInfo.Arguments = string.Concat(progToRun, " ", path, " ", mpt_file, " ", mask_choice);
            //Console.WriteLine("Processing...");
            proc.Start();

            StreamReader sReader = proc.StandardOutput;
            string[] output = sReader.ReadToEnd().Split(splitter);

            return output;
        }

        private string[] window_mask(string path, string mpt_file, string x_min, string x_max, string y_min, string y_max)
        {
            string pt1 = python_script_location;
            string progToRun = pt1 + "\\window_masker.py";
            char[] splitter = { '\r' };

            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            // call hello.py to concatenate passed parameters

            //"from tools import *; print(mpt_data(path=r'C:\Users\cjang\Desktop\Kyler_Speed_Circuit\data\\',data = ['DE_49_8_30.mpt']).guess_and_plot(mask = [1000018.6, 28]))"
            proc.StartInfo.Arguments = string.Concat(progToRun, " ", path, " ", mpt_file, " ", x_min, " ", x_max, " ", y_min, " ", y_max);
            //Console.WriteLine("Processing...");
            proc.Start();

            StreamReader sReader = proc.StandardOutput;
            string[] output = sReader.ReadToEnd().Split(splitter);

            return output;
        }

        private string[] window_mpt(string path, string mpt_file, string x_min, string x_max, string y_min, string y_max)
        {
            string pt1 = python_script_location;
            string progToRun = pt1 + "\\window_df.py";
            char[] splitter = { '\r' };

            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            // call hello.py to concatenate passed parameters

            //"from tools import *; print(mpt_data(path=r'C:\Users\cjang\Desktop\Kyler_Speed_Circuit\data\\',data = ['DE_49_8_30.mpt']).guess_and_plot(mask = [1000018.6, 28]))"
            proc.StartInfo.Arguments = string.Concat(progToRun, " ", path, " ", mpt_file, " ", x_min, " ", x_max, " ", y_min, " ", y_max);
            //Console.WriteLine("Processing...");
            proc.Start();

            StreamReader sReader = proc.StandardOutput;
            string[] output = sReader.ReadToEnd().Split(splitter);

            return output;
        }

        private string[] window_masked_fit(string path, string mpt_file, string x_min, string x_max, string y_min, string y_max)
        {
            string pt1 = python_script_location;
            string progToRun = pt1 + "\\window_guesser.py";
            char[] splitter = { '\r' };

            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            // call hello.py to concatenate passed parameters

            //"from tools import *; print(mpt_data(path=r'C:\Users\cjang\Desktop\Kyler_Speed_Circuit\data\\',data = ['DE_49_8_30.mpt']).guess_and_plot(mask = [1000018.6, 28]))"
            proc.StartInfo.Arguments = string.Concat(progToRun, " ", path, " ", mpt_file, " ", x_min, " ", x_max, " ", y_min, " ", y_max);
            //Console.WriteLine("Processing...");
            proc.Start();

            StreamReader sReader = proc.StandardOutput;
            string[] output = sReader.ReadToEnd().Split(splitter);

            return output;
        }


        private void btnFit_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void lin_kk_mask_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void Fitter_Load(object sender, EventArgs e)
        {
        }

        private void pathbutton_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folder_label.Text = folderBrowserDialog1.SelectedPath.ToString();
                file_display.Items.Clear();
                
                string box_form = string.Join(", ", path_listing(folderBrowserDialog1.SelectedPath));
                if (box_form.Length > 0)
                {

                    path_list_box.Text = box_form;

                    foreach (string single_file in path_listing(folderBrowserDialog1.SelectedPath))
                    {
                        file_display.Items.Add(single_file);
                    }
                }
                else
                {
                    MessageBox.Show("Empty folder or a bad directory");
                }
                pre_test = path_list_box.Text.Split(' ');
                post_test = new string[pre_test.Length];
                
            }
        }

        private void nvyquist_mousewheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                var xMin = xAxis.ScaleView.ViewMinimum;
                var xMax = xAxis.ScaleView.ViewMaximum;
                var yMin = yAxis.ScaleView.ViewMinimum;
                var yMax = yAxis.ScaleView.ViewMaximum;
                
                if (e.Delta < 0) // Scrolled down.
                {
                    xAxis.ScaleView.ZoomReset();
                    yAxis.ScaleView.ZoomReset();
                    //MessageBox.Show("RESET" + xMin.ToString());
                    //essageBox.Show("RESET" + xMax.ToString());
                }
                else if (e.Delta > 0) // Scrolled up.
                {
                    
                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 8;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 8;
                    var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 8;
                    var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 8;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    yAxis.ScaleView.Zoom(posYStart, posYFinish);
                    //MessageBox.Show("zoom in: " + posXStart.ToString());
                    //MessageBox.Show("zoom in: " + posXFinish.ToString());
                }
                
            }
            catch { }

        }

        public event EventHandler Customize
        {
            add
            {

            }
            remove
            {

            }
        }
        private void file_select_Click(object sender, EventArgs e)
        {
            freq.Clear();
            re.Clear();
            im.Clear();
            fit_re.Clear();
            fit_im.Clear();

            
            foreach (var series in nvyquist.Series)
            {
                series.Points.Clear();
            }
            try
            {
                if (string.Join("", post_test).Length>0)
                {
                    path_list_box.Text = string.Join("", post_test);
                }
                

                string mpt_file = Regex.Replace(file_display.SelectedItem.ToString(), @"\t|\n|\r", "");
                string raw_path = folder_label.Text;
                file_display_label.Text = mpt_file;
                string[] output = mpt_dataframe(raw_path, file_display.SelectedItem.ToString());
                List<string> pre = output.ToList();
                pre.RemoveAt(0);
                string box_form = string.Join("", pre);


                foreach (string sgl in pre)
                {
                    Queue<Double> dbl_prep = new Queue<double>();
                    //MessageBox.Show("NEW LINE");
                    foreach (var word in sgl.Split(' '))
                    {
                        if (word.Length > 5)
                        {
                            //MessageBox.Show(word);
                            dbl_prep.Enqueue(Convert.ToDouble(word));
                        }
                    }
                    if (dbl_prep.Count == 3)
                    {
                        freq.Add(dbl_prep.Dequeue());
                        re.Add(dbl_prep.Dequeue());
                        im.Add(dbl_prep.Dequeue());
                    }
                    else
                    {
                        foreach (var word in dbl_prep)
                        {
                            MessageBox.Show("VALUE: " + word.ToString());
                        }
                    }
                }
                //MessageBox.Show(freq.Count().ToString());
                //string to_return = String.Join(string.Join("", freq), string.Join("", re), string.Join("", im));
                //MessageBox.Show(to_return);
                dataframe_box.Text = box_form;
                error_box.Text = "";


                //nvyquist.ChartAreas[0].CursorX.IsUserEnabled = true;
                //nvyquist.ChartAreas[0].CursorY.IsUserEnabled = true;



                //nvyquist.ChartAreas[0].RecalculateAxesScale();






                nvyquist.MouseWheel += nvyquist_mousewheel;
                nvyquist.Series[0].Points.DataBindXY(re, im);
                //nvyquist.Series.Add("Nyvquist").Points.DataBindXY(re, im);
                //nvyquist.ForeColor = Color.ForestGreen;
                //MessageBox.Show(nvyquist.ChartAreas[0].AxisX.Maximum.ToString());
                //MessageBox.Show(nvyquist.ChartAreas[0].AxisY.Maximum.ToString());
                
                x_min.Text = Math.Round(nvyquist.ChartAreas[0].AxisX.Minimum, 2).ToString();
                x_max.Text = Math.Round(nvyquist.ChartAreas[0].AxisX.Maximum, 2).ToString();
                y_min.Text = Math.Round(nvyquist.ChartAreas[0].AxisY.Minimum, 2).ToString();
                y_max.Text = Math.Round(nvyquist.ChartAreas[0].AxisY.Maximum, 2).ToString();
                
            }
            catch (NullReferenceException nullspot)
            {
                MessageBox.Show("Select a Value");
            }


        }

        private void masker1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string mpt_file = file_display.SelectedItem.ToString();
                string raw_path = folder_label.Text;
                string[] output = masker(raw_path, mpt_file, "1");
              
                string box_form = string.Join("", output);
                mask_limits.Text = output[0].ToString();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Bad path or Bad MPT file; Please Select a folder and a file");
            }
        }

        private void masker2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string mpt_file = file_display.SelectedItem.ToString();
                string raw_path = folder_label.Text;
                string[] output = masker(raw_path, mpt_file, "2");
                string box_form = string.Join(", ", output);
                mask_limits.Text = box_form;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Bad path or Bad MPT file; Please Select a folder and a file");
            }
        }

        private void masker3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string mpt_file = file_display.SelectedItem.ToString();
                string raw_path = folder_label.Text;
                string[] output = masker(raw_path, mpt_file, "3");
                string box_form = string.Join(", ", output);
                mask_limits.Text = box_form;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Bad path or Bad MPT file; Please Select a folder and a file");
            }
        }
        private void entire_fit_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void window_masker_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (window_masker.Checked == true)
                {
                    nvyquist.ChartAreas[0].AxisX.Minimum = double.NaN;
                    nvyquist.ChartAreas[0].AxisY.Minimum = double.NaN;

                    nvyquist.ChartAreas[0].RecalculateAxesScale();
                    x_min.Text = nvyquist.ChartAreas[0].AxisX.ScaleView.ViewMinimum.ToString();
                    x_max.Text = nvyquist.ChartAreas[0].AxisX.ScaleView.ViewMaximum.ToString();
                    y_min.Text = nvyquist.ChartAreas[0].AxisY.ScaleView.ViewMinimum.ToString();
                    y_max.Text = nvyquist.ChartAreas[0].AxisY.ScaleView.ViewMaximum.ToString();
                }
                
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please Fill in all four values to obtain a valid window.");
            }
        }


        private void masker_fit(string raw_path, string mpt_file, string masker_choice)
        {
            string[] masked_df = masked_mpt(raw_path, mpt_file, masker_choice);
            string[] output = masked_guesser(raw_path, mpt_file, masker_choice);
            List<string> pre = output.ToList();


            string fit_label = pre[1];
            string fit_coeffs = pre[2];
            //Console.WriteLine(fit_label);
            pre.RemoveRange(0, 3);

            string box_form = string.Join("", pre);
            error_box.Text = box_form;
            string box_form_mpt = string.Join("", masked_df);
            dataframe_box.Text = box_form_mpt;
            //string to_export = string.Join(" ", fit_label, fit_coeffs);
            fit_coeffs_box.AppendText(fit_coeffs);
            //fit_coeffs_box.AppendText(fit_coeffs);

            foreach (string sgl in pre)
            {
                Queue<Double> dbl_prep = new Queue<double>();

                foreach (var word in sgl.Split(','))
                {
                    if (word.Length > 1)
                    {
                        //MessageBox.Show("WORD: " + word);
                        dbl_prep.Enqueue(Convert.ToDouble(word));
                    }
                }
                if (dbl_prep.Count == 2)
                {
                    fit_re.Add(dbl_prep.Dequeue());
                    fit_im.Add(dbl_prep.Dequeue());
                }
                else
                {
                    foreach (var word in dbl_prep)
                    {
                        MessageBox.Show("Ununsual Value: " + word.ToString());
                    }
                }
            }
        }

        private void window_masker_fit(string raw_path, string mpt_file, string xmin, string xmax, string ymin, string ymax)
        {
            //string[] masked_df = masked_mpt(raw_path, mpt_file, masker_choice);
            string[] output = window_masked_fit(raw_path, mpt_file, xmin, xmax, ymin, ymax);
            //string[] masked_df = window_mask
            List<string> pre = output.ToList();


            string fit_label = pre[1];
            string fit_coeffs = pre[2];

            pre.RemoveRange(0, 3);

            //Console.WriteLine(fit_label);
            //Console.WriteLine(fit_coeffs);

            string box_form = string.Join("", pre);
            error_box.Text = box_form;
            //string to_export = string.Join(" ", fit_label, fit_coeffs);
            fit_coeffs_box.AppendText(fit_label);
            //fit_coeffs_box.AppendText(fit_coeffs);

            foreach (string sgl in pre)
            {
                Queue<Double> dbl_prep = new Queue<double>();

                foreach (var word in sgl.Split(','))
                {
                    if (word.Length > 1)
                    {
                        //MessageBox.Show("WORD: " + word);
                        dbl_prep.Enqueue(Convert.ToDouble(word));
                    }
                }
                if (dbl_prep.Count == 2)
                {
                    fit_re.Add(dbl_prep.Dequeue());
                    fit_im.Add(dbl_prep.Dequeue());
                }
                else
                {
                    foreach (var word in dbl_prep)
                    {
                        MessageBox.Show("Ununsual Value: " + word.ToString());
                    }
                }
            }
            
        }


        private void fit_function_Click(object sender, EventArgs e)
        {
            try
            {
                
                string mpt_file = file_display_label.Text;
                string raw_path = folder_label.Text;
                fit_re.Clear();
                fit_im.Clear();

                


                int bad_index = new int();
                List<String> lst_fits = new List<String>();
                foreach (string str in fit_coeffs_box.Lines)
                {
                    lst_fits.Add(str);
                }

                foreach (string str in lst_fits)
                {
                    if (str.Contains(mpt_file))
                    {
                        MessageBox.Show("HERERERERER");
                        bad_index = lst_fits.IndexOf(mpt_file);
                        MessageBox.Show(bad_index.ToString());
                    }
                }

                if (entire_fit.Checked == true)
                {
                    MessageBox.Show(mpt_file, raw_path);
                    //string[] output = masked_guesser(raw_path, mpt_file, "1");
                    //string[] masked_df = masked_mpt(raw_path, mpt_file, "1");
                    string[] output = guesser(raw_path, mpt_file);
                    List<string> pre = output.ToList();
                    try
                    {
                        string fit_label = pre[0];
                        string fit_coeffs = pre[1];

                        pre.RemoveRange(0, 3);
                        
                        
                        //string to_export = string.Join(" ", fit_label, fit_coeffs);
                        //fit_coeffs_box.AppendText(fit_label);
                        fit_coeffs_box.AppendText(fit_coeffs);

                        //MessageBox.Show(mpt_file);
                        

                    }
                    catch (ArgumentOutOfRangeException range_error)
                    {
                        MessageBox.Show("The guessing Function has not completed; Please fit a mask or fit again and wait");
                    }
                    

                    string box_form = string.Join("", pre);
                    error_box.Text = box_form;
                    

                    foreach (string sgl in pre)
                    {
                        Queue<Double> dbl_prep = new Queue<double>();
                        //MessageBox.Show("NEW LINE
                        //MessageBox.Show(sgl);
                        foreach (var word in sgl.Split(','))
                        {
                            if (word.Length > 1)
                            {
                                //MessageBox.Show(word);
                                dbl_prep.Enqueue(Convert.ToDouble(word));
                            }
                        }
                        if (dbl_prep.Count == 2)
                        {
                            fit_re.Add(dbl_prep.Dequeue());
                            fit_im.Add(dbl_prep.Dequeue());
                        }
                        else
                        {
                            foreach (var word in dbl_prep)
                            {
                                MessageBox.Show("Ununsual Value: " + word.ToString());
                            }
                        }
                    }
                    
                }
                else if (masker1.Checked == true)
                {
                    masker_fit(raw_path, mpt_file, "1");
                }
                else if (masker2.Checked == true)
                {
                    masker_fit(raw_path, mpt_file, "2");
                }
                else if (masker3.Checked == true)
                {
                    masker_fit(raw_path, mpt_file, "3");
                }
                else if (window_masker.Checked == true)
                {
                    nvyquist.ChartAreas[0].AxisX.Minimum = double.NaN;
                    nvyquist.ChartAreas[0].AxisY.Minimum = double.NaN;

                    nvyquist.ChartAreas[0].RecalculateAxesScale();
                    x_min.Text = nvyquist.ChartAreas[0].AxisX.ScaleView.ViewMinimum.ToString();
                    x_max.Text = nvyquist.ChartAreas[0].AxisX.ScaleView.ViewMaximum.ToString();
                    y_min.Text = nvyquist.ChartAreas[0].AxisY.ScaleView.ViewMinimum.ToString();
                    y_max.Text = nvyquist.ChartAreas[0].AxisY.ScaleView.ViewMaximum.ToString();
                    window_masker_fit(raw_path, mpt_file, x_min.Text, x_max.Text, y_min.Text, y_max.Text);

                }
                else
                {
                    MessageBox.Show("Bad Masking Choice");
                }

                if (fit_re.Count() > 0)
                {
                    if (string.Join("", post_test).Length > 0)
                    {
                        for (int counter = 0; counter < post_test.Length; counter++)
                        {
                            string striped = Regex.Replace(Regex.Replace(post_test[counter], @"\n", ""), @",", "");
                            if (striped == mpt_file)
                            {
                                post_test[counter] = ((striped + " FITTED\n"));
                            }
                            else
                            {
                                post_test[counter] = ((striped + "\n"));
                            }
                        }
                        temp = string.Join("", post_test);
                        path_list_box.Text = string.Join("", post_test);
                    }
                    else
                    {
                        for (int counter = 0; counter < pre_test.Length; counter++)
                        {
                            string striped = Regex.Replace(Regex.Replace(pre_test[counter], @"\n", ""), @",", "");
                            if (striped == mpt_file)
                            {
                                post_test[counter] = ((striped + " FITTED\n"));
                            }
                            else
                            {
                                post_test[counter] = ((striped + "\n"));
                            }
                        }
                        temp = string.Join("", post_test);
                        path_list_box.Text = string.Join("", post_test);
                    }
                }

                MessageBox.Show("Completed Fitting of " + fit_re.Count().ToString() + " values.");
                //string to_return = String.Join(string.Join("", freq), string.Join("", re), string.Join("", im));
                //MessageBox.Show(to_return);
                //dataframe_box.Text = box_form;
                
                
                

                nvyquist.Series[1].Points.DataBindXY(fit_re, fit_im);
                //nvyquist.Series.Add("Fitted_Nyvquist").Points.DataBindXY(fit_re, fit_im);

                //nvyquist.ForeColor = Color.ForestGreen;

            }
            catch (Exception wide)
            {
                MessageBox.Show("Bad path or Bad MPT file; Please Select a folder and a file;");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void nvyquist_Click(object sender, EventArgs e)
        {
            
        }

        private void export_button_Click(object sender, EventArgs e)
        {
            List<string> to_export = new List<string>();
            to_export.Add("index, file, fit_R, fit_Rs, fit_n, fit_Q, fit_R2, fit_n2, fit_Q2, fit_n3, fit_Q3");

            foreach (string st0 in fit_coeffs_box.Lines)
            {
                string st1 = Regex.Replace(st0, "              ", ", ");
                string st2 = Regex.Replace(st1, "             ", ", ");
                string st3 = Regex.Replace(st2, "            ", ", ");
                string st4 = Regex.Replace(st3, "           ", ", ");
                string st5 = Regex.Replace(st4, "          ", ", ");
                string st6 = Regex.Replace(st5, "         ", ", ");
                string st7 = Regex.Replace(st6, "        ", ", ");
                string st8 = Regex.Replace(st7, "       ", ", ");
                string st9 = Regex.Replace(st8, "      ", ", ");
                string s10 = Regex.Replace(st9, "     ", ", ");
                string s11 = Regex.Replace(s10, "    ", ", ");
                string s12 = Regex.Replace(s11, "   ", ", ");
                string new_line = Regex.Replace(s12, "  ", ", ");
                string striped_line = Regex.Replace(new_line, "/", "");
                //MessageBox.Show(striped_line.Substring(0));
                //MessageBox.Show(striped_line);
                to_export.Add(striped_line);
            }
            

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                File.WriteAllLines(filename, to_export);
            }
            
        }

        
    }
}