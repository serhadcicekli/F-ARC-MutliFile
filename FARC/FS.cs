//File System code
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FARC2
{
    class FS
    {

        byte[] fxd(string nm)
        {
            //This function takes a string and converts it to a 256 byte array. If the length of the string is less than 256, it assigns 0 to the extra bytes.
            byte[] tmp = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                if (i < nm.Length)
                {
                    try
                    {
                        tmp[i] = Convert.ToByte(nm[i]);
                    }
                    catch (Exception)
                    {

                        tmp[i] = 0;
                    }

                }
                else
                {
                    tmp[i] = 0;
                }
            }
            return tmp;
        }
        byte[] getnum(int nm)
        {
            //this function converts an integer to a 4 byte array of bytes
            return BitConverter.GetBytes(nm);
        }
        uint getnuma(byte[] byt)
        {
            //this function converts a 4 byte array of bytes to integer 
            return BitConverter.ToUInt32(byt, 0);
        }
        Stream outputStream;    //Output file stream for extracting .farc media
        bool mrgo = false;  //A boolean indicating whether merge or file writing operations is enabled
        void openmerge(string outputFile)
        {
            //configures the program for file merging and writing
            if (!mrgo)
            {
                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }
                mrgo = true;
                outputStream = File.OpenWrite(outputFile);
            }
        }
        void writebytes(byte[] wrt)
        {
            //appends the byte string of the end of the file if file merge is on
            if (mrgo)
            {
                outputStream.Write(wrt, 0, wrt.Length);

            }
        }
        void mrgfl(string inputFile)
        {
            //Appends the contents of another file to file without using RAM
            if (mrgo)
            {
                using (Stream inputStream = File.OpenRead(inputFile))
                {
                    inputStream.CopyTo(outputStream);
                    inputStream.Close();
                }
            }
        }
        void closemerge()
        {
            //closes the configuration for file operations
            mrgo = false;
            outputStream.Close();
        }

        public void createmedia(string saveloc, string dir)
        {
            openmerge(saveloc);          //configures the program for file merging and writing
            writebytes(Encoding.UTF8.GetBytes("F-ARC$"));   //creates F-ARC$ signature
            int fln = Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Length;   //gets the number of files  
            writebytes(getnum(fln));    //writes file count to media file in converted format
            FileInfo fi;   
            
            foreach (string pth in Directory.GetFiles(dir,"*",SearchOption.AllDirectories)) //adds the length of all files in order to the media file
            {
                    Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
                    fi = new FileInfo(pth);
                    writebytes(getnum((int)fi.Length));

            }
            fi = null;
            foreach (string pth in Directory.GetFiles(dir, "*", SearchOption.AllDirectories))
            {
                    Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
                    writebytes(fxd(pth.Split('\\').Last())); //appends the filename to the media file
                    mrgfl(pth); //appends the file content to the media file

            }
            closemerge();
            MessageBox.Show("Media created");
        }
        string getfname(byte[] fnc)
        {
            //Converts 256 bytes array to string
            string tfn = "";
            if (fnc.Length == 256)
            {
                for (int i = 0; i < 256; i++)
                {
                    if (fnc[i] != 0)
                    {
                        tfn = tfn + Convert.ToChar(fnc[i]);
                    }
                }
                return tfn;
            }
            else
            {
                return "";
            }
        }
        public void extractmedia(string getloc, string savedir)
        {
            byte[] bufferremainder;
            Stream tmpfstream;
            byte[] fname = new byte[256];
            byte[] buffer = new byte[512];
            byte[] adrllng = new byte[4];
            byte[] tmpa = new byte[4];
            uint[] flln;    
            using (FileStream fs = new FileStream(getloc, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(6, SeekOrigin.Begin); //skips the signature
                fs.Read(adrllng, 0, 4);         //reads the number of files
                uint filec = getnuma(adrllng);
                flln = new uint[filec]; //creates an array of unsigned integers to contain the file lengths
                for (int i = 0; i < filec; i++)//writes file lengths to array
                {
                    fs.Read(adrllng, 0, 4);
                    flln[i] = getnuma(adrllng);
                }
                for (int x = 0; x < filec; x++)
                {
                    fs.Read(fname, 0, 256); //reads the filename
                    tmpfstream = File.OpenWrite(savedir + "\\" + getfname(fname)); //opens a filestream by filename
                    for (int i = 0; i < flln[x] / (uint)512; i++) //splits file content into 512-byte buffers
                    {
                        fs.Read(buffer, 0, 512);    //Reads 512 bytes and puts them in buffer
                        tmpfstream.Write(buffer, 0, 512);   //writes buffer to media file
                    }
                    bufferremainder = new byte[flln[x] % (uint)512];    //After the file is divided into 512-byte buffers, it reads the remaining part and writes it to the media file.
                    fs.Read(bufferremainder, 0, bufferremainder.Length);
                    tmpfstream.Write(bufferremainder, 0, bufferremainder.Length);
                    bufferremainder = null;
                    tmpfstream.Close();
            
                }
            }
        }
    }
}
