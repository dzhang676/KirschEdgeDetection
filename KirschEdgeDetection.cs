using System;
using static System.Console;
using System.IO;
using System.Linq;

namespace Bme121.Pa3
{
    ///<StudentPlan>Biomedical Engineering</StudentPlan>
	///<StudentDept>Department of Systems Design Engineering</StudentDept>
	///<StudentInst>University of Waterloo</StudentInst>
	///<StudentName>Zhang,David</StudentName>
	///<StudentUserID>dbzhang</StudentUserID>
	///<StudentAcknowledgements>
	///I declare that, except as acknowledged below, this is my original work.
	///Acknowledged contributions of others:
	///</StudentAcknowledgements>
    
    static partial class Program
    {
        static void Main( )
        {
            string inputFile  = @"21_training.csv";
            string outputFile = @"21_training_edges.csv";
            int height;  // image height (number of rows)
            int width;  // image width (number of columns)
            Color[ , ] inImage;
            Color[ , ] outImage;
            
            // Read the input image from its csv file.
			// TO DO:
              FileStream ReadImage = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
              StreamReader sr = new StreamReader (ReadImage);
              
              //Reading in the height and width from first two lines
              height = int.Parse(sr.ReadLine());
              width = int.Parse(sr.ReadLine());
              
              inImage = new Color [height,width];
              
              for (int i = 0; i <height; i++)
              {
				 
				 //Storing the lines into an array and splitting them
				 string[] line= sr.ReadLine().Split(','); 
				 

				  for (int j=0; j < width; j++)
				  {
					 int a = int.Parse(line[j*4]);
					 int r = int.Parse(line[j*4 +1]);
					 int g = int.Parse(line[j*4 +2]);
					 int b = int.Parse(line[j*4 +3]);		
					 
					 //Using the FromArgb method to put values into inImage
					 inImage[i,j]= Color.FromArgb(a,r,g,b); 
				  }	 
		      }
                       
            // Generate the output image using Kirsch edge detection.
               outImage = new Color [height,width];

              for (int k = 0; k<height; k++)
               {
                for (int l=0; l <width; l++)
				   { 
					   //Using detection on all except for edge pixels
				   
					   if (k==0 || (k == height -1) || (l==0) || (l == width -1))
					   {
						   outImage[k,l]=inImage[k,l];
					   }else
					   {
					
					   outImage[k,l]= GetKirschEdgeValue(inImage[k-1, l-1], inImage[k-1,l], inImage[k-1, l+1], 
														 inImage[k,l-1],    inImage[k, l],  inImage[k, l+1],
														 inImage[k+1, l-1], inImage[k+1, l],inImage[k+1, l+1]); 
				   
						}
				  }
				   
			   }
            
            // Write the output image into the csv file
			FileStream WriteFile = new FileStream(outputFile, FileMode.Open, FileAccess.Write);
			StreamWriter sw = new StreamWriter (WriteFile); 
			
			
			sw.WriteLine(height);
			sw.WriteLine(width);
			//sw.WriteLine("test");
			
			for (int m=0; m<height ;m++)
			{
				string[] words = new string[width];
				for (int n=0; n<width; n++)
				{
					
					words[n] = outImage[m,n].A + "," + outImage[m,n].R + "," + outImage[m,n].G + "," + outImage[m,n].B;
					
					
				}
				
				sw.WriteLine( string.Join(",", words) );
			}
			
			//Disposes the files
			sw.Dispose();
			sr.Dispose();
			WriteFile.Dispose();
			ReadImage.Dispose();

        }
        
        // This method computes the Kirsch edge-detection value for pixel color
        // at the centre location given the centre-location pixel color and the
        // colors of its eight neighbours.  These are numbered as follows.
        // The resulting color has the same alpha as the centre pixel, 
        // and Kirsch edge-detection intensities which are computed separately
        // for each of the red, green, and blue components using its eight neighbours.
        // c1 c2 c3
        // c4    c5
        // c6 c7 c8
        
        static Color GetKirschEdgeValue( 
            Color c1, Color c2,     Color c3, 
            Color c4, Color centre, Color c5, 
            Color c6, Color c7,     Color c8 )
        {

			int r = GetKirschEdgeValue(c1.R, c2.R, c3.R, c4.R, c5.R, c6.R, c7.R, c8.R);
			int g = GetKirschEdgeValue(c1.G, c2.G, c3.G, c4.G, c5.G, c6.G, c7.G, c8.G);
			int b = GetKirschEdgeValue(c1.B, c2.B, c3.B, c4.B, c5.B, c6.B, c7.B, c8.B);
			
            // TO DO: (Replace the following line.)
            
            return ( Color.FromArgb(centre.A ,r, g, b ));
        }
        
        // This method computes the Kirsch edge-detection value for pixel intensity
        // at the centre location given the pixel intensities of the eight neighbours.
        // These are numbered as follows.
        // i1 i2 i3
        // i4    i5
        // i6 i7 i8
        static int GetKirschEdgeValue( 
            int i1, int i2, int i3, 
            int i4,         int i5, 
            int i6, int i7, int i8 )
        {
		  //Calculates the individual Kirsch edge detection values
		  int Kirsch1= (i1*5 + i2*5 + i3*5 + i4*-3 + i5*-3 + i6*-3 + i7*-3 + i8*-3);
		  int Kirsch2= (i1*-3 + i2*5 + i3*5 + i4*-3 + i5*5 + i6*-3 + i7*-3 + i8*-3);
		  int Kirsch3= (i1*-3 + i2*-3+ i3*5 + i4*-3 + i5*5 + i6*-3 + i7*-3 + i8*5);
		  int Kirsch4= (i1*-3 + i2*-3+ i3*-3 + i4*-3 + i5*5 + i6*-3 + i7*5 + i8*5);
		  int Kirsch5= (i1*-3 + i2*-3+ i3*-3 + i4*-3 + i5*-3 + i6*5 + i7*5 + i8*5);
		  int Kirsch6= (i1*-3 + i2*-3+ i3*-3 + i4*5 + i5*-3 + i6*5 + i7*5 + i8*-3);
		  int Kirsch7= (i1*5 + i2*-3+ i3*-3 + i4*5 + i5*-3 + i6*5 + i7*-3 + i8*-3);
		  int Kirsch8= (i1*5 + i2*5+ i3*-3 + i4*5 + i5*-3 + i6*-3 + i7*-3 + i8*-3);
		  
            // TO DO: (Replace the following line)
            
            //Finds the largest Kirsch value, if it is outside of range set to closest parameter inside range of 0<=x<=255
            int MaxKirsch = Math.Max(Math.Max(Math.Max(Kirsch1, Kirsch2), Math.Max(Kirsch3, Kirsch4)),Math.Max(Math.Max(Kirsch5, Kirsch6), Math.Max(Kirsch7, Kirsch8)));
            
            if (MaxKirsch < 0)
            {
				return 0;
			}
            else if (MaxKirsch >255)
            {
				return 255;
			}
            else
            {
				return MaxKirsch;
			}
        }
    }
    
    // Implementation of part of System.Drawing.Color.
    // This is needed because .Net Core doesn't seem to include the assembly 
    // containing System.Drawing.Color even though docs.microsoft.com claims 
    // it is part of the .Net Core API.
    struct Color
    {
        int alpha;
        int red;
        int green;
        int blue;
        
        public int A { get { return alpha; } }
        public int R { get { return red;   } }
        public int G { get { return green; } }
        public int B { get { return blue;  } }
        
        public static Color FromArgb( int alpha, int red, int green, int blue )
        {
            Color result = new Color( );
            result.alpha = alpha;
            result.red   = red;
            result.green = green;
            result.blue  = blue;
            return result;
        }
    }
}
