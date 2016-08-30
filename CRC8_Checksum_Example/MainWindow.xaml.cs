using System;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace CRC8_Checksum_Example
{
    public partial class MainWindow : Window
    {

        // CRC 8 lookup table
        byte[] CRC_8_TABLE =
        {
              0, 94,188,226, 97, 63,221,131,194,156,126, 32,163,253, 31, 65,
            157,195, 33,127,252,162, 64, 30, 95,  1,227,189, 62, 96,130,220,
             35,125,159,193, 66, 28,254,160,225,191, 93,  3,128,222, 60, 98,
            190,224,  2, 92,223,129, 99, 61,124, 34,192,158, 29, 67,161,255,
             70, 24,250,164, 39,121,155,197,132,218, 56,102,229,187, 89,  7,
            219,133,103, 57,186,228,  6, 88, 25, 71,165,251,120, 38,196,154,
            101, 59,217,135,  4, 90,184,230,167,249, 27, 69,198,152,122, 36,
            248,166, 68, 26,153,199, 37,123, 58,100,134,216, 91,  5,231,185,
            140,210, 48,110,237,179, 81, 15, 78, 16,242,172, 47,113,147,205,
             17, 79,173,243,112, 46,204,146,211,141,111, 49,178,236, 14, 80,
            175,241, 19, 77,206,144,114, 44,109, 51,209,143, 12, 82,176,238,
             50,108,142,208, 83, 13,239,177,240,174, 76, 18,145,207, 45,115,
            202,148,118, 40,171,245, 23, 73,  8, 86,180,234,105, 55,213,139,
             87,  9,235,181, 54,104,138,212,149,203, 41,119,244,170, 72, 22,
            233,183, 85, 11,136,214, 52,106, 43,117,151,201, 74, 20,246,168,
            116, 42,200,150, 21, 75,169,247,182,232, 10, 84,215,137,107, 53
        };

        public MainWindow()
        {
            InitializeComponent();

            setupTextBoxes();
        }

        // 
        // Set initial values for the examples
        public void setupTextBoxes()
        {
            this.textBox_ExampleOneDescription_StartDelimiter.Text = "<";
            this.textBox_ExampleOneDescription_LengthMSB.Text = "L1";
            this.textBox_ExampleOneDescription_LengthLSB.Text = "L0";
            this.textBox_ExampleOneDescription_CMD.Text = "CMD";
            this.textBox_ExampleOneDescription_D0.Text = "D0";
            this.textBox_ExampleOneDescription_D1.Text = "D1";
            this.textBox_ExampleOneDescription_D2.Text = "D2";
            this.textBox_ExampleOneDescription_CRC.Text = "CRC";
            this.textBox_ExampleOneDescription_EndDelimiter.Text = ">";

            this.textBox_ExampleOne_StartDelimiter.Text = "60";     // Decimal 60 = ASCII "<"
            this.textBox_ExampleOne_LengthMSB.Text = "0";           // Length is 4
            this.textBox_ExampleOne_LengthLSB.Text = "4";           //    1 CMD byte and 3 Data bytes
            this.textBox_ExampleOne_CMD.Text = "145";               // SET Integration time is 0x91
            this.textBox_ExampleOne_D0.Text = "D0";                 // Data bytes to be calculated later
            this.textBox_ExampleOne_D1.Text = "D1";                 // 
            this.textBox_ExampleOne_D2.Text = "D2";                 // 
            this.textBox_ExampleOne_CRC.Text = "CRC";               // CRC to be calculated later
            this.textBox_ExampleOne_EndDelimiter.Text = "62";       // Decimal 62 = ASCII ">"
        }

        //
        // Simple regular expression for textbox validation to only allow numeric characters
        private void IsTextAllowed(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]"); //regex that matches disallowed text
            e.Handled = regex.IsMatch(e.Text);
            //return !regex.IsMatch(text);
        }

        private void button_ExampleOne_Calculate_Click(object sender, RoutedEventArgs e)
        {
            // Protection from a null value
            if (textBox_ExampleOne_Payload == null)
                textBox_ExampleOne_Payload.Text = "0";

            // Validate and bound our input to a limit of 16,777,216
            if (int.Parse(textBox_ExampleOne_Payload.Text) >= 16777216 )
            {
                textBox_ExampleOne_Payload.Text = "16777215";
            }

            // Convert our integer value to a byte array for packet and checksum
            byte[] bytes = BitConverter.GetBytes(int.Parse(textBox_ExampleOne_Payload.Text));

            // NOTE the endian and truncating the 32-bit conversion down to 24-bit
            textBox_ExampleOne_D0.Text = bytes[0].ToString();   
            textBox_ExampleOne_D1.Text = bytes[1].ToString();
            textBox_ExampleOne_D2.Text = bytes[2].ToString();

            // Create our packet 
            byte[] crcPacket =  {
                (byte)int.Parse(textBox_ExampleOne_LengthMSB.Text),
                (byte)int.Parse(textBox_ExampleOne_LengthLSB.Text),
                (byte)int.Parse(textBox_ExampleOne_CMD.Text),
                (byte)int.Parse(textBox_ExampleOne_D0.Text),
                (byte)int.Parse(textBox_ExampleOne_D1.Text),
                (byte)int.Parse(textBox_ExampleOne_D2.Text)
            };

            textBox_ExampleOne_Resultant_CRC.Text = Calc_CRC_8(crcPacket, 6).ToString() ;
        }

        byte Calc_CRC_8(byte[] DataArray, int Length)
        {
            int i;
            byte CRC;

            CRC = 0;

            for (i=0; i<Length; i++)
		        CRC = CRC_8_TABLE[CRC ^ DataArray[i]];

	        return CRC;
        }
    }
}
