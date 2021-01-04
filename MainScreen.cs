﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Dynamic;
using System.Configuration;

namespace AtMoS3
{
    public partial class MainScreen : Form
    {
        private string version;

        public MainScreen()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Displays the About form.
            About myNewForm = new About();
            myNewForm.Show();
        }

#pragma warning disable IDE1006 // Naming Styles
        private void Form1_Load(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            //this.ControlBox = false;
            //blankLabels();
            lblStatus.Text = "Awaiting start.";
            //backgroundWorker1.RunWorkerAsync();
            bwGetSystemTime.RunWorkerAsync();

            //  We set a version here so that the features of the program can be configured depending on
            //  which organisation is using the program.  The active features are determined by the value 
            //  of "User" in the app.config file..

            version = ConfigurationManager.AppSettings["User"];
            configVersion();

    }

        private void configVersion()
        {
            //  Here we make changes to the program features at runtime depending on which organisation
            //  is using the program.  The active features are determined by the value 
            //  of "User" in the app.config file.
            if (version == "Southern Cross University")
            {
                this.Text = "AtMoS - SCU";
                startToolStripMenuItem.Visible = false;
                pumpToolStripMenuItem.Visible = false;
                tabControl1.TabPages.Remove(Sample);
            }
            else
            {
                this.Text = "AtMoS - Illawarra Coatings";
                startToolStripMenuItem2.Visible = false;
                toolStripStatusLabel1.Text = "AtMoS - Licensed to Illawarra Coatings.";
            }

            //  This code simply removes the label text used during design.
            lblSystemTime.Text = "";
            lblTemperature.Text = "";
            lblHumidity.Text = "";
            lblPressure.Text = "";
            lblNOAE.Text = "";
            lblNOWE.Text = "";
            lblDataFileLocation.Text = "";
            lblNO2AE.Text = "";
            lblNO2WE.Text = "";
            lblNOConc.Text = "";
            lblNO2Conc.Text = "";

        }

        private void closingEvent(object sender, FormClosingEventArgs e)
        {
            //stopPump();
            System.Windows.Forms.Application.Exit();
            Environment.Exit(0);
            this.Close();
        }
        
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  This section of code creates a new datafile and writes its location to a label so other parts of the
            //  program can reference the file location and write data.  The code also now loads column heading info
            //  into the datafile.
            //  Code works correctly...
            Stream _myStream;
            try
            {
                if (txtExpDesc.Text == "")
                {
                    MessageBox.Show("I'd prefer that you enter a description for the experiment before continuing.", "Oops...",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                }
                else
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.ShowDialog();
                    _myStream = saveFileDialog1.OpenFile();
                    lblDataFileLocation.Text = saveFileDialog1.FileName;
                    _myStream.Close();

                    //  Now write the datafile name, Experiment description and Column headings to the datafile.
                    //  Append the .csv extension to the Experiment number to generate the filename.
                    string logFileName = lblDataFileLocation.Text + ".csv";

                    //  Here we construct the datastrings.  The \r ending is important to ensure that each data entry begins on a new line.
                    string expDesc = txtExpDesc.Text + "\r";
                    string dataHeadings = "Time stamp" + "," + "Atmospheric Pressure" + "," + "Temperature" + "," + "Humidity" + "," + "NOAE Volts" + "," + "NOWE Volts" + "," + "NO2AE Volts" + "," + "NO2WE Volts" + "\r";

                    // Write the datastring to the file "_logFileName".
                    using (StreamWriter outputFile = File.AppendText(logFileName))
                        try
                        {
                            outputFile.WriteLine(expDesc);
                            outputFile.WriteLine(dataHeadings);
                            outputFile.Close();
                        }
                        catch
                        {

                        }
                }
            }
            catch
            {
            }
            finally
            {
                //  Once the datafile has been created we make a number of menu items visible.  This prevents a user from
                //  trying to start the program when the datafile has not been created.
                calibrationToolStripMenuItem.Visible = true;
                aquisitionToolStripMenuItem.Visible = true;
                pumpToolStripMenuItem.Visible = true;
            }
        }

        private void write2DataFile()
        {
            //  This is the method we use to write data to the datafile.
            //  Format the timestamp so that date and time are within the same cell when loaded into Excel.
            string timeStamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            //  Append the .csv extension to the Experiment number to generate the filename.
            string _logFileName = lblDataFileLocation.Text + ".csv";

            //  Here we construct the datastring.  The \r ending is important to ensure that each data entry begins on a new line.
            string _data2Write = timeStamp + "," + lblPressure.Text + "," + lblTemperature.Text + "," + lblHumidity.Text + "," + lblNOAE.Text + "," + lblNOWE.Text + "," + lblNO2AE.Text + "," + lblNO2WE.Text + "," + "\r";

            // Write the datastring to the file "_logFileName".
            using (StreamWriter outputFile = File.AppendText(_logFileName))
                try
                {
                    outputFile.WriteLine(_data2Write);
                    outputFile.Close();
                }
                catch
                {
                }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Let's first of all make certain that there is a datafile created to store experiment output.
            try
            {
                if (lblDataFileLocation.Text == "")
                {
                    MessageBox.Show("You need to have created a datafile before you can start data aquisition.", "Oops...",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                }
            }
            catch
            {
            }

        }

        private void startPump()
        {
            //  The method calls a python script whose function is to energise the relay connected to the
            //  usb pump thereby switching it on.
            string python = @"/usr/bin/python";
            string pythonStartPump = @"/home/pi/Programs/Python/pumpSwitching/switchON.py";
            Process _myProcess = new Process();
            ProcessStartInfo _myProcessStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = false,
                FileName = python,
                Arguments = pythonStartPump
            };

            _myProcess.StartInfo = _myProcessStartInfo;
            _myProcess.Start();
        }


        private void startToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            startPump();
        }

        private void stopPump()
        {
            //  The method calls a python script whose function is to deenergise the relay connected to the
            //  usb pump thereby switching it off.
            string python = @"/usr/bin/python";
            string pythonStartPump = @"/home/pi/Programs/Python/pumpSwitching/switchOFF.py";
            Process _myProcessOff = new Process();
            ProcessStartInfo _myProcessStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = false,
                FileName = python,
                Arguments = pythonStartPump
            };

            _myProcessOff.StartInfo = _myProcessStartInfo;
            _myProcessOff.Start();
        }

        private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            stopPump();
        }

        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bwGetGasPulsed.RunWorkerAsync();
        }



        private void setlblStatusTextSafely(string text)
        {
            //  We use the InvokeRequired method to prevent a  "Cross thread operation not valid".  This error occurs when we try to 
            //  call a Windows Forms control from a thread that didn't create that control.  We can pass a text value from the calling 
            //  function.
            //  
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new MethodInvoker(delegate { lblStatus.Text = text; }));
            }
            else
            {
                lblStatus.Text = text;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopPump();
            System.Windows.Forms.Application.Exit();
            Environment.Exit(0);
            this.Close();
        }



        private void tmrWrite2File_Tick(object sender, EventArgs e)
        {
            write2DataFile();
        }

        private void dataCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmrWrite2File.Start();
        }

        private void bwGetClimate_DoWork(object sender, DoWorkEventArgs e)
        {
            //  Backgroundworker 1 is used to update the climate information on the form.  The getClimate() method handles
            //  possible "Cross thread operation not valid" errors that may occur when we try to call a Windows form control
            //  from a thread that didn't create that control.

            //  This thread is started at form_load and doesn't have a stop function.
            while (true)
                getClimate();
        }

        private void getClimate()
        {
            //  This getClimate() method calls a python script that reads temperature, humidity and atmospheric pressure
            //  information from a Bosch BME280 sensor.  The python script average 5 readings with a 0.1 seconds delay
            //  between each reading.  This delay prevents issues with a reading not being completed before the 
            //  commencement of the next measurement.  The python script then returns the new measurement. 

            //  Here we use the AddMilliseconds function to help create the sleep function between gas analyses.
            //  Doing it this way results in a more accurate delay than Thread.Sleep().  We determine the current
            //  time and then add the appropriate delay the the finishTime variable.  Further down this method,
            //  there is a while loop that checks the current time against the set finishTime and loops until
            //  finishTime and therefore the delay is completed.  The only issue is that the program must complete
            //  one cycle after the delay is set for the new delay to be active...this is however the same when using
            //  Thread.Sleep().  The only way to change this is to stop the analysis and restart.  This delay triggers
            //  the getClimate() method every second.  This value can be changed in the Parameters > Sampling Rates
            //  tab control if this tab is active for the organisation.

            int delayTime = Convert.ToInt32(txtClimateUpdatedInterval.Text) * 1000;
            DateTime finishTime = (DateTime.Now).AddMilliseconds(delayTime);

            string python = @"/usr/bin/python";
            string climate = @"/home/pi/Adafruit_BME280_Library/examples/bme280.py";
            Process _myProcess = new Process();
            ProcessStartInfo _myProcessStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                FileName = python,
                Arguments = climate
            };

            _myProcess.StartInfo = _myProcessStartInfo;
            _myProcess.Start();

            StreamReader _myStreamReader = _myProcess.StandardOutput;
            string _temp = _myStreamReader.ReadLine();

            //  We use the InvokeRequired method to prevent a  "Cross thread operation not valid".This error occurs when we try to
            //  call a Windows Forms control from a thread that didn't create that control.  We can pass a text value from the calling 
            //  function.
            if (lblTemperature.InvokeRequired)
            {
                lblTemperature.Invoke(new MethodInvoker(delegate { lblTemperature.Text = _temp; }));
            }
            else
            {
                lblTemperature.Text = _temp;
            }

            string _press = _myStreamReader.ReadLine();
            if (lblPressure.InvokeRequired)
            {
                lblPressure.Invoke(new MethodInvoker(delegate { lblPressure.Text = _press; }));
            }
            else
            {
                lblPressure.Text = _press;
            }

            string _humid = _myStreamReader.ReadLine();
            if (lblHumidity.InvokeRequired)
            {
                lblHumidity.Invoke(new MethodInvoker(delegate { lblHumidity.Text = _humid; }));
            }
            else
            {
                lblHumidity.Text = _humid;
            }

            //  Lets publish the climate data to Thingsboard just to check and see if the python script works correctly.

            //  Yes...this is working correctly.  All that needs to happen now is to also include the other values of interest 
            //  such as electrode outputs etc. and move the code to the backgroundworker3() method, after the getGas() method.
            /*
            string args2 = string.Format(@"/home/pi/AtMoS3P/publishData.py {0} {1} {2} ", _temp, _humid, _press);

            try
            {
                Process publish = new Process();
                ProcessStartInfo publishProcessStartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    FileName = python,
                    Arguments = args2
                };

                publish.StartInfo = publishProcessStartInfo;
                publish.Start();
                publish.WaitForExit();
            }
            catch
            {

            }
            */

            //This is the loop described above that creates the delay similiar to Thread.Sleep().
            while (DateTime.Now < finishTime)
            {
                //  Create a loop
            }

        }

        private void bwGetSystemTime_DoWork(object sender, DoWorkEventArgs e)
        {
            //  Backgroundworker 2 is used to update the system time on the form.  System time is used both as a check that the 
            //  program has not been caught in an unresponsive loop but also as the source of the timestamp information for
            //  writing to the datafile.  The timestamp for publishing to the cloud is derived from the called python script
            //
            //  We use the InvokeRequired method to prevent a  "Cross thread operation not valid".This error occurs when we try to
            //  call a Windows Forms control from a thread that didn't create that control.  We can pass a text value from the calling 
            //  function.

            //  This thread is started at form_load and doesn't have a stop function.
            while (true)
            {
                if (lblSystemTime.InvokeRequired)
                {
                    lblSystemTime.Invoke(new MethodInvoker(delegate { lblSystemTime.Text = DateTime.Now.ToString(); }));
                    Thread.Sleep(100);
                }
                else
                {
                    lblSystemTime.Text = DateTime.Now.ToString();
                    Thread.Sleep(100);
                }
            }
        }

        private void bwGetGasPulsed_DoWork(object sender, DoWorkEventArgs e)
        {
            //pumpAuto();
            while (true)
            {
                //  Backgroundworker 3 is used to do pulse measurement of the chamber atmosphere.  We start by opening
                //  the solenoid valve attached to the sensor system exit line and start the pump.  We purge the  
                //  sensor system for a timeperiod determined by the value of txtPurgeTime.  At the end of that 
                //  time, we call the getGas() method to do the actual analysis.  The pump continues for a short time 
                //  after the end of the analysis cycle.  We then stop the pump, close the solenoid and put the 
                //  system to sleep.
                //  
                //  Here we use the AddMilliseconds function to help create the sleep function between gas analyses.
                //  Doing it this way results in a more accurate delay than Thread.Sleep().  We determine the current
                //  time and then add the appropriate delay the the finishTime variable.  Further down this method,
                //  there is a while loop that checks the current time against the set finishTime and loops until
                //  finishTime and therefore the delay is completed.  The only issue is that the program must complete
                //  one cycle after the delay is set for the new delay to be active...this is however the same whrn using
                //  Thread.Sleep().  The only way to change this is to stop the analysis and restart.

                //DateTime startTimeBW3 = DateTime.Now;
                DateTime finishTimeBW3 = (DateTime.Now).AddMilliseconds(Convert.ToInt32(txtSleepTime.Text) * 1000 + 2000);
                DateTime purgeFinish = (DateTime.Now).AddMilliseconds(Convert.ToInt32(txtPurgeTime.Text) * 1000 + 1000);
                //DateTime samplingFinish = (DateTime.Now).AddMilliseconds(Convert.ToInt32(txtSamplingTime.Text) * 1000);

                setlblStatusTextSafely("Gas hood solenoid energised.");
                openSolenoid();
                DateTime pumpStartDelay = (DateTime.Now).AddMilliseconds(1000);
                setlblStatusTextSafely("Sensor purge cycle started.");
                while (DateTime.Now < pumpStartDelay)
                {
                    //  Create a loop
                }
                startPump();

                while (DateTime.Now < purgeFinish)
                {
                    //  Loop
                }

                setlblStatusTextSafely("Analysing chamber atmospheric composition");
                getGasPulsed();

                setlblStatusTextSafely("Sleeping...waiting for next cycle");
                stopPump();
                DateTime pumpStopDelay = (DateTime.Now).AddMilliseconds(1000);
                while (DateTime.Now < pumpStopDelay)
                {
                    //  Create a loop
                }
                closeSolenoid();

                //  Now publish the data to Thingsboard
                //publishData();
                publish2Adafruit();

                //  Write data to datafile
                write2DataFile();
                
                //This is the loop described above that creates the delay similiar to Thread.Sleep().
                while (DateTime.Now < finishTimeBW3)
                {
                    //  Create a loop
                }
            }


        }  

        private void getGasPulsed()
        {
            /*  This getGas() method calls a python script that reads electrode values from the South Coast Science
             *  DFE which has an series of Alphasense electrochemical sensors attached to an AFE.
             *  
             *  The script takes one reading per second (1Hz) for a period of time determined by the value of
             *  txtSamplingTime.Text.  A delay is built into the script to force the 1Hz measurement period.
             *  
             *  Once the sampling is completed, the script calculates the average value for the electrode outputs
             *  and returns that value to this program.
             *  
             *  There were some issues with running this program initially.  When the version of python was 
             *  changed to python3, the error disappeared.
             *  
            */
            string python = @"/usr/bin/python3";
            string args = string.Format(@"/home/pi/Adafruit_Python_ADS1x15/Gas.py {0}", txtSamplingTime.Text);

            Process getgas = new Process();
            ProcessStartInfo publishProcessStartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    FileName = python,
                    Arguments = args
                };

                getgas.StartInfo = publishProcessStartInfo;
                getgas.Start();
                //getgas.WaitForExit();
            
            StreamReader _myStreamReader = getgas.StandardOutput;
            string NO_WE = _myStreamReader.ReadLine();

            //  We use the InvokeRequired method to prevent a  "Cross thread operation not valid".This error occurs when we try to
            //  call a Windows Forms control from a thread that didn't create that control.  We can pass a text value from the calling 
            //  function.
            if (lblNOWE.InvokeRequired)
            {
                lblNOWE.Invoke(new MethodInvoker(delegate { lblNOWE.Text = NO_WE; }));
            }
            else
            {
                lblNOWE.Text = NO_WE;
            }

            string NO_AE = _myStreamReader.ReadLine();
            if (lblNOAE.InvokeRequired)
            {
                lblNOAE.Invoke(new MethodInvoker(delegate { lblNOAE.Text = NO_AE; }));
            }
            else
            {
                lblNOAE.Text = NO_AE;
            }

            string NO2_WE = _myStreamReader.ReadLine();
            if (lblNO2WE.InvokeRequired)
            {
                lblNO2WE.Invoke(new MethodInvoker(delegate { lblNO2WE.Text = NO2_WE; }));
            }
            else
            {
                lblNO2WE.Text = NO2_WE;
            }

            string NO2_AE = _myStreamReader.ReadLine();
            if (lblNO2AE.InvokeRequired)
            {
                lblNO2AE.Invoke(new MethodInvoker(delegate { lblNO2AE.Text = NO2_AE; }));
            }
            else
            {
                lblNO2AE.Text = NO2_AE;
            }
        }

        private void startToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            bwGetClimate.RunWorkerAsync();
        }

       

        private void pulsedSamplingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bwGetGasPulsed.RunWorkerAsync();
        }

        private void electrodeOffsetMeasurementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bwCalculateElectrodeOffsets.RunWorkerAsync();
        }

        

        private void electrodeOffsetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bwCalculateElectrodeOffsets.RunWorkerAsync();
        }

        private void bwCalculateElectrodeOffsets_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            double sumNOWE = 0;
            double sumNOAE = 0;
            double sumNO2WE = 0;
            double sumNO2AE = 0;
            double avgNOWE = 0;
            double avgNOAE = 0;
            double avgNO2WE = 0;
            double avgNO2AE = 0;


            setlblStatusTextSafely("Calculating electrode offsets.");

            startPump();
            while (true)
            {

                DateTime finishTimeBW5 = (DateTime.Now).AddMilliseconds(2000);
                getGasContinuous();
                sumNOWE += Convert.ToDouble(lblNOWE.Text);
                sumNOAE += Convert.ToDouble(lblNOAE.Text);
                sumNO2WE += Convert.ToDouble(lblNO2WE.Text);
                sumNO2AE += Convert.ToDouble(lblNO2AE.Text);

                count += 1;

                avgNOWE = Math.Round((sumNOWE / count),5);
                avgNOAE = Math.Round((sumNOAE / count),5);
                avgNO2WE = Math.Round((sumNO2WE / count),5);
                avgNO2AE = Math.Round((sumNO2AE / count),5);
                /*
                //  Need to fix this as it will throw a cross thread error.
                txtNOWEOffset.Text = avgNOWE.ToString("#.#####");
                txtNOAEOffset.Text = avgNOAE.ToString("#.#####");
                txtNO2WEOffset.Text = avgNO2WE.ToString("#.#####");
                txtNO2AEOffset.Text = avgNO2AE.ToString("#.#####");
                */

                if (txtNOWEOffset.InvokeRequired)
                {
                    txtNOWEOffset.Invoke(new MethodInvoker(delegate { txtNOWEOffset.Text = avgNOWE.ToString("#.#####"); }));
                    txtNOAEOffset.Invoke(new MethodInvoker(delegate { txtNOAEOffset.Text = avgNOAE.ToString("#.#####"); }));
                    txtNO2WEOffset.Invoke(new MethodInvoker(delegate { txtNO2WEOffset.Text = avgNO2WE.ToString("#.#####"); }));
                    txtNO2AEOffset.Invoke(new MethodInvoker(delegate { txtNO2AEOffset.Text = avgNO2AE.ToString("#.#####"); }));
                }
                else
                {
                    txtNOWEOffset.Text = txtNOWEOffset.Text = avgNOWE.ToString("#.#####");
                    txtNOAEOffset.Text = txtNOAEOffset.Text = avgNOAE.ToString("#.#####");
                    txtNO2WEOffset.Text = txtNO2WEOffset.Text = avgNO2WE.ToString("#.#####");
                    txtNO2AEOffset.Text = txtNO2AEOffset.Text = avgNO2AE.ToString("#.#####");
                }

                //publishData();
                write2DataFile();

                while (DateTime.Now < finishTimeBW5)
                {
                    //  Create a loop
                }
            }

        }

        private void continuousSamplingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bwGetGasContinuous.RunWorkerAsync();
        }

        private void bwGetGasContinuous_DoWork(object sender, DoWorkEventArgs e)
        {
            stopPump();
            while (true)
            {
                DateTime finishTimeBW6 = (DateTime.Now).AddMilliseconds(1000);
                getGasContinuous();

                //publishData();
                //publish2Adafruit();
                write2DataFile();

                //This is the loop described above that creates the delay similiar to Thread.Sleep().
                while (DateTime.Now < finishTimeBW6)
                {
                    //  Create a loop
                }

            }
  
        }

        private void getGasContinuous()
        {
            /*  This getGas() method calls a python script that reads electrode values from the South Coast Science
             *  DFE which has an series of Alphasense electrochemical sensors attached to an AFE.
             *  
             *  The script takes one reading per second (1Hz) for a period of time determined by the value of
             *  txtSamplingTime.Text.  A delay is built into the script to force the 1Hz measurement period.
             *  
             *  Once the sampling is completed, the script calculates the average value for the electrode outputs
             *  and returns that value to this program.
             *  
             *  There were some issues with running this program initially.  When the version of python was 
             *  changed to python3, the error disappeared.
             *  
            */
            string python = @"/usr/bin/python3";
            string args = @"/home/pi/Adafruit_Python_ADS1x15/Gas2.py";

            Process getgas = new Process();
            ProcessStartInfo publishProcessStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = python,
                Arguments = args
            };

            getgas.StartInfo = publishProcessStartInfo;
            getgas.Start();
            //getgas.WaitForExit();

            StreamReader _myStreamReader = getgas.StandardOutput;
            string NO_WE = _myStreamReader.ReadLine();

            //  We use the InvokeRequired method to prevent a  "Cross thread operation not valid".This error occurs when we try to
            //  call a Windows Forms control from a thread that didn't create that control.  We can pass a text value from the calling 
            //  function.
            if (lblNOWE.InvokeRequired)
            {
                lblNOWE.Invoke(new MethodInvoker(delegate { lblNOWE.Text = NO_WE; }));
            }
            else
            {
                lblNOWE.Text = NO_WE;
            }

            string NO_AE = _myStreamReader.ReadLine();
            if (lblNOAE.InvokeRequired)
            {
                lblNOAE.Invoke(new MethodInvoker(delegate { lblNOAE.Text = NO_AE; }));
            }
            else
            {
                lblNOAE.Text = NO_AE;
            }

            string NO2_WE = _myStreamReader.ReadLine();
            if (lblNO2WE.InvokeRequired)
            {
                lblNO2WE.Invoke(new MethodInvoker(delegate { lblNO2WE.Text = NO2_WE; }));
            }
            else
            {
                lblNO2WE.Text = NO2_WE;
            }

            string NO2_AE = _myStreamReader.ReadLine();
            if (lblNO2AE.InvokeRequired)
            {
                lblNO2AE.Invoke(new MethodInvoker(delegate { lblNO2AE.Text = NO2_AE; }));
            }
            else
            {
                lblNO2AE.Text = NO2_AE;
            }

        }

        private void publishElectrodeValues(string objNOWE, string NO_WE, string objNOAE, string NO_AE, string objNO2WE, string NO2_WE, string objNO2AE, string NO2_AE)
        {
            /*  This method publishes the electrode values to the form.
             *  We use the InvokeRequired method to prevent a "Cross thread operation not valid".This error occurs when we try to
             *  call a Windows Forms control from a thread that didn't create that control.  We can pass a text value from the calling
             *  function using this method.
             * 
             *  We do a test to determine if InvokeRequired is required for the first of the objects to update.  If it is required
             *  then it will be required for all the objects being updated.  This saves multiple if/else conditions.                
            */

            if (lblNOWE.InvokeRequired)
            {
                lblNOWE.Invoke(new MethodInvoker(delegate { objNOWE = NO_WE; }));
                lblNOAE.Invoke(new MethodInvoker(delegate { objNOAE = NO_AE; }));
                lblNO2WE.Invoke(new MethodInvoker(delegate { objNO2WE = NO2_WE; }));
                lblNO2AE.Invoke(new MethodInvoker(delegate { objNO2AE = NO2_AE; }));
            }
            else
            {
                objNOWE = NO_WE;
                objNOAE = NO_AE;
                objNO2WE = NO2_WE;
                objNO2AE = NO2_AE;
            }
                       
        }

        private void startBaselineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bwCalculateElectrodeOffsets.RunWorkerAsync();
        }

        private void stopBaselineToolStripMenuItem_Click(object sender, EventArgs e)
        {

            
        }

        private void publish2Adafruit()
        {
            //  Lets publish the climate data to Adafruit just to check and see if the python script works correctly.

            //  Yes...this is working correctly.  All that needs to happen now is to also include the other values of interest 
            //  such as electrode outputs etc. and move the code to the backgroundworker3() method, after the getGas() method.
            string python = @"/usr/bin/python";
            string args3 = string.Format(@"/home/pi/Programs/Python/publish2Cloud/publish2Cloud.py {0} {1} {2} {3} {4} {5} {6}", lblTemperature.Text, lblHumidity.Text, lblPressure.Text, lblNOAE.Text, lblNOWE.Text, lblNO2AE.Text, lblNO2WE.Text);

            try
            {
                Process publish = new Process();
                ProcessStartInfo publishProcessStartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    FileName = python,
                    Arguments = args3
                };

                publish.StartInfo = publishProcessStartInfo;
                publish.Start();
                publish.WaitForExit();
            }
            catch
            {
            }
        }


        private void publishResultsToCloudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bwPublish2Adafruit.RunWorkerAsync();
        }

        private void bwPublish2Adafruit_DoWork(object sender, DoWorkEventArgs e)
        {
            publish2Adafruit();

            //  Only publish to Adafruit every 15 seconds to prevent throttling errors on io.adafruit.com
            //delayLoop(15);
            DateTime nextPublishTime = (DateTime.Now).AddMilliseconds(15000);
            while (DateTime.Now < nextPublishTime)
            {
                //  Create a loop
            }
        }

        private void openSolenoid()
        {
            //  The method calls a python script whose function is to energise the relay connected to the
            //  calibration hood solenoid valve thereby switching it on.
            string python = @"/usr/bin/python";
            string pythonStartPump = @"/home/pi/Programs/Python/solenoidOperation/openSolenoid.py";
            Process energiseSolenoid = new Process();
            ProcessStartInfo energiseSolenoidStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = false,
                FileName = python,
                Arguments = pythonStartPump
            };

            energiseSolenoid.StartInfo = energiseSolenoidStartInfo;
            energiseSolenoid.Start();
        }

        private void closeSolenoid()
        {
            //  The method calls a python script whose function is to deenergise the relay connected to the
            //  calibration hood solenoid valve thereby switching it off.
            string python = @"/usr/bin/python";
            string pythonStartPump = @"/home/pi/Programs/Python/solenoidOperation/closeSolenoid.py";
            Process deenergiseSolenoid = new Process();
            ProcessStartInfo deenergiseSolenoidStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = false,
                FileName = python,
                Arguments = pythonStartPump
            };

            deenergiseSolenoid.StartInfo = deenergiseSolenoidStartInfo;
            deenergiseSolenoid.Start();
        }

        private void delayLoop(Int32 delay)
        {
            //  Create a delay in the program using the .AddMilliseconds function.  Obtain the delay from various 
            //  textboxes.
            DateTime delayFinishTime = (DateTime.Now).AddMilliseconds(delay * 1000);
            while (DateTime.Now < delayFinishTime)
            {
                //  Create a loop
            }
        }
    }



    /*  atmos4
     *  
     *  04/01/2021 1532 - Increase finish times for purge and sampling in getGasPulsed to accoiunt for solenoid delay.
     *  04/01/2021 1238 - Add gas hood solenoid energised advisory...reduce delay to 1 second.
     *  04/01/2021 1227 - delayLoop does not work so add directly to getGasPulsed function.
     *  04/01/2021 1219 - Use delayLoop in getGasPulsed to create delay between solenoid opening/closing and pump starting/stopping.
     *  
     *  */


        
    
}
