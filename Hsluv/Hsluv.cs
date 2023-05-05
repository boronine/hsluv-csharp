using System;
using System.Collections.Generic;

namespace Hsluv
{
    public class HsluvConverter
    {

        // Rgb <--> Hsluv(p)

        public static IList<double> HsluvToRgb(IList<double> tuple)
        {
            HsluvConverterFast conv = new HsluvConverterFast();
            conv.hsluv_h = tuple[0];
            conv.hsluv_s = tuple[1];
            conv.hsluv_l = tuple[2];
            conv.hsluvToRgb();
            return new double[]
            {
                conv.rgb_r,
                conv.rgb_g,
                conv.rgb_b
            };
        }

        public static IList<double> RgbToHsluv(IList<double> tuple)
        {
            HsluvConverterFast conv = new HsluvConverterFast();
            conv.rgb_r = tuple[0];
            conv.rgb_g = tuple[1];
            conv.rgb_b = tuple[2];
            conv.rgbToHsluv();
            return new double[]
            {
                conv.hsluv_h,
                conv.hsluv_s,
                conv.hsluv_l
            };
        }

        public static IList<double> HpluvToRgb(IList<double> tuple)
        {
            HsluvConverterFast conv = new HsluvConverterFast();
            conv.hpluv_h = tuple[0];
            conv.hpluv_p = tuple[1];
            conv.hpluv_l = tuple[2];
            conv.hpluvToRgb();
            return new double[]
            {
                conv.rgb_r,
                conv.rgb_g,
                conv.rgb_b
            };
        }

        public static IList<double> RgbToHpluv(IList<double> tuple)
        {
            HsluvConverterFast conv = new HsluvConverterFast();
            conv.rgb_r = tuple[0];
            conv.rgb_g = tuple[1];
            conv.rgb_b = tuple[2];
            conv.rgbToHpluv();
            return new double[]
            {
                conv.hpluv_h,
                conv.hpluv_p,
                conv.hpluv_l
            };
        }

        // Hex

        public static string HsluvToHex(IList<double> tuple)
        {
            HsluvConverterFast conv = new HsluvConverterFast();
            conv.hsluv_h = tuple[0];
            conv.hsluv_s = tuple[1];
            conv.hsluv_l = tuple[2];
            conv.hsluvToHex();
            return conv.hex;
        }

        public static string HpluvToHex(IList<double> tuple)
        {
            HsluvConverterFast conv = new HsluvConverterFast();
            conv.hpluv_h = tuple[0];
            conv.hpluv_p = tuple[1];
            conv.hpluv_l = tuple[2];
            conv.hpluvToHex();
            return conv.hex;
        }

        public static IList<double> HexToHsluv(string s)
        {
            HsluvConverterFast conv = new HsluvConverterFast();
            conv.hex = s;
            conv.hexToHsluv();
            return new double[]
            {
                conv.hsluv_h,
                conv.hsluv_s,
                conv.hsluv_l
            };
        }

        public static IList<double> HexToHpluv(string s)
        {
            HsluvConverterFast conv = new HsluvConverterFast();
            conv.hex = s;
            conv.hexToHpluv();
            return new double[]
            {
                conv.hpluv_h,
                conv.hpluv_p,
                conv.hpluv_l
            };
        }
    }

    public class HsluvConverterFast

    {
        public static string hexChars = "0123456789abcdef";
        public static double refY = 1.0;
        public static double refU = 0.19783000664283;
        public static double refV = 0.46831999493879;
        public static double kappa = 903.2962962;
        public static double epsilon = 0.0088564516;
        public static double m_r0 = 3.240969941904521;
        public static double m_r1 = -1.537383177570093;
        public static double m_r2 = -0.498610760293;
        public static double m_g0 = -0.96924363628087;
        public static double m_g1 = 1.87596750150772;
        public static double m_g2 = 0.041555057407175;
        public static double m_b0 = 0.055630079696993;
        public static double m_b1 = -0.20397695888897;
        public static double m_b2 = 1.056971514242878;


        public string hex;
        public double rgb_r;
        public double rgb_g;
        public double rgb_b;
        public double xyz_x;
        public double xyz_y;
        public double xyz_z;
        public double luv_l;
        public double luv_u;
        public double luv_v;
        public double lch_l;
        public double lch_c;
        public double lch_h;
        public double hsluv_h;
        public double hsluv_s;
        public double hsluv_l;
        public double hpluv_h;
        public double hpluv_p;
        public double hpluv_l;
        public double r0s;
        public double r0i;
        public double r1s;
        public double r1i;
        public double g0s;
        public double g0i;
        public double g1s;
        public double g1i;
        public double b0s;
        public double b0i;
        public double b1s;
        public double b1i;

        public HsluvConverterFast()
        {

        }

        public static double fromLinear(double c)
        {
            if (c <= 0.0031308)
            {
                return 12.92 * c;
            }
            else
            {
                return (1.055 * Math.Pow(c, 0.416666666666666685)) - 0.055;
            }

        }


        public static double toLinear(double c)
        {
            if (c > 0.04045)
            {
                return Math.Pow((c + 0.055) / 1.055, 2.4);
            }
            else
            {
                return c / 12.92;
            }

        }


        public static double yToL(double Y)
        {
            if (Y <= HsluvConverterFast.epsilon)
            {
                return Y / HsluvConverterFast.refY * HsluvConverterFast.kappa;
            }
            else
            {
                return ((116 * Math.Pow((((Y / HsluvConverterFast.refY))), ((0.333333333333333315)))) - 16);
            }
        }


        public static double lToY(double L)
        {
            if ((L <= 8))
            {
                return ((HsluvConverterFast.refY * L) / HsluvConverterFast.kappa);
            }
            else
            {
                return (HsluvConverterFast.refY * Math.Pow((((((L + 16)) / 116))), ((3))));
            }
        }


        public static string rgbChannelToHex(double chan)
        {
            int c = Convert.ToInt16(Math.Round(chan * 255.0));
            int digit2 = c % 16;
            int digit1 = (c - digit2) / 16;
            return Char.ToString(HsluvConverterFast.hexChars[digit1]) + Char.ToString(HsluvConverterFast.hexChars[digit2]);
        }


        public static double hexToRgbChannel(string hex, int offset)
        {
            int digit1 = HsluvConverterFast.hexChars.IndexOf(hex[offset], 0);
            int digit2 = HsluvConverterFast.hexChars.IndexOf(hex[offset + 1], 0);
            int n = digit1 * 16 + digit2;
            return n / 255.0;

        }


        public static double distanceFromOriginAngle(double slope, double intercept, double angle)
        {
            double d = intercept / (Math.Sin(angle) - (slope * Math.Cos(angle)));
            if (d < 0)
            {
                return Double.PositiveInfinity;
            }
            else
            {
                return d;
            }

        }


        public static double distanceFromOrigin(double slope, double intercept)
        {
            return (Math.Abs(((intercept))) / Math.Sqrt((((Math.Pow(((slope)), ((2))) + 1)))));

        }


        public static double min6(double f1, double f2, double f3, double f4, double f5, double f6)
        {
            return Math.Min(((f1)), ((Math.Min(((f2)), ((Math.Min(((f3)), ((Math.Min(((f4)), ((Math.Min(((f5)), ((f6)))))))))))))));
        }



        public virtual void rgbToHex()
        {
            this.hex = "#" + HsluvConverterFast.rgbChannelToHex(this.rgb_r) + HsluvConverterFast.rgbChannelToHex(this.rgb_g) + HsluvConverterFast.rgbChannelToHex(this.rgb_b);
        }


        public virtual void hexToRgb()
        {
            this.hex = this.hex.ToLowerInvariant();
            this.rgb_r = HsluvConverterFast.hexToRgbChannel(this.hex, 1);
            this.rgb_g = HsluvConverterFast.hexToRgbChannel(this.hex, 3);
            this.rgb_b = HsluvConverterFast.hexToRgbChannel(this.hex, 5);
        }


        public virtual void xyzToRgb()
        {
            this.rgb_r = HsluvConverterFast.fromLinear((((HsluvConverterFast.m_r0 * this.xyz_x) + (HsluvConverterFast.m_r1 * this.xyz_y)) + (HsluvConverterFast.m_r2 * this.xyz_z)));
            this.rgb_g = HsluvConverterFast.fromLinear((((HsluvConverterFast.m_g0 * this.xyz_x) + (HsluvConverterFast.m_g1 * this.xyz_y)) + (HsluvConverterFast.m_g2 * this.xyz_z)));
            this.rgb_b = HsluvConverterFast.fromLinear((((HsluvConverterFast.m_b0 * this.xyz_x) + (HsluvConverterFast.m_b1 * this.xyz_y)) + (HsluvConverterFast.m_b2 * this.xyz_z)));
        }


        public virtual void rgbToXyz()
        {
            double lr = HsluvConverterFast.toLinear(this.rgb_r);
            double lg = HsluvConverterFast.toLinear(this.rgb_g);
            double lb = HsluvConverterFast.toLinear(this.rgb_b);
            this.xyz_x = (((0.41239079926595 * lr) + (0.35758433938387 * lg)) + (0.18048078840183 * lb));
            this.xyz_y = (((0.21263900587151 * lr) + (0.71516867876775 * lg)) + (0.072192315360733 * lb));
            this.xyz_z = (((0.019330818715591 * lr) + (0.11919477979462 * lg)) + (0.95053215224966 * lb));
        }


        public virtual void xyzToLuv()
        {
            double divider = ((this.xyz_x + (15 * this.xyz_y)) + (3 * this.xyz_z));
            double varU = (4 * this.xyz_x);
            double varV = (9 * this.xyz_y);
            if ((divider != 0))
            {
                varU /= divider;
                varV /= divider;
            }
            else
            {
                varU = Double.NaN;
                varV = Double.NaN;
            }

            this.luv_l = HsluvConverterFast.yToL(this.xyz_y);
            if ((this.luv_l == 0))
            {
                this.luv_u = ((0));
                this.luv_v = ((0));
            }
            else
            {
                this.luv_u = ((13 * this.luv_l) * ((varU - HsluvConverterFast.refU)));
                this.luv_v = ((13 * this.luv_l) * ((varV - HsluvConverterFast.refV)));
            }
        }


        public virtual void luvToXyz()
        {
            if ((this.luv_l == 0))
            {
                this.xyz_x = ((0));
                this.xyz_y = ((0));
                this.xyz_z = ((0));
                return;
            }

            double varU = ((this.luv_u / ((13 * this.luv_l))) + HsluvConverterFast.refU);
            double varV = ((this.luv_v / ((13 * this.luv_l))) + HsluvConverterFast.refV);
            this.xyz_y = HsluvConverterFast.lToY(this.luv_l);
            this.xyz_x = (0 - (((9 * this.xyz_y) * varU) / (((((varU - 4)) * varV) - (varU * varV)))));
            this.xyz_z = (((((9 * this.xyz_y) - ((15 * varV) * this.xyz_y)) - (varV * this.xyz_x))) / ((3 * varV)));
        }


        public virtual void luvToLch()
        {
            this.lch_l = this.luv_l;
            this.lch_c = Math.Sqrt(((((this.luv_u * this.luv_u) + (this.luv_v * this.luv_v)))));
            if ((this.lch_c < 0.00000001))
            {
                this.lch_h = ((0));
            }
            else
            {
                double Hrad = Math.Atan2(((this.luv_v)), ((this.luv_u)));
                this.lch_h = ((Hrad * 180.0) / Math.PI);
                if ((this.lch_h < 0))
                {
                    this.lch_h = (360 + this.lch_h);
                }

            }
        }


        public virtual void lchToLuv()
        {
            double Hrad = ((this.lch_h / 180.0) * Math.PI);
            this.luv_l = this.lch_l;
            this.luv_u = (Math.Cos(((Hrad))) * this.lch_c);
            this.luv_v = (Math.Sin(((Hrad))) * this.lch_c);
        }


        public virtual void calculateBoundingLines(double l)
        {
            double sub1 = (Math.Pow((((l + 16))), ((3))) / 1560896);
            double sub2 = (((sub1 > HsluvConverterFast.epsilon)) ? (sub1) : ((l / HsluvConverterFast.kappa)));
            double s1r = (sub2 * (((284517 * HsluvConverterFast.m_r0) - (94839 * HsluvConverterFast.m_r2))));
            double s2r = (sub2 * ((((838422 * HsluvConverterFast.m_r2) + (769860 * HsluvConverterFast.m_r1)) + (731718 * HsluvConverterFast.m_r0))));
            double s3r = (sub2 * (((632260 * HsluvConverterFast.m_r2) - (126452 * HsluvConverterFast.m_r1))));
            double s1g = (sub2 * (((284517 * HsluvConverterFast.m_g0) - (94839 * HsluvConverterFast.m_g2))));
            double s2g = (sub2 * ((((838422 * HsluvConverterFast.m_g2) + (769860 * HsluvConverterFast.m_g1)) + (731718 * HsluvConverterFast.m_g0))));
            double s3g = (sub2 * (((632260 * HsluvConverterFast.m_g2) - (126452 * HsluvConverterFast.m_g1))));
            double s1b = (sub2 * (((284517 * HsluvConverterFast.m_b0) - (94839 * HsluvConverterFast.m_b2))));
            double s2b = (sub2 * ((((838422 * HsluvConverterFast.m_b2) + (769860 * HsluvConverterFast.m_b1)) + (731718 * HsluvConverterFast.m_b0))));
            double s3b = (sub2 * (((632260 * HsluvConverterFast.m_b2) - (126452 * HsluvConverterFast.m_b1))));
            this.r0s = (s1r / s3r);
            this.r0i = ((s2r * l) / s3r);
            this.r1s = (s1r / ((s3r + 126452)));
            this.r1i = ((((s2r - 769860)) * l) / ((s3r + 126452)));
            this.g0s = (s1g / s3g);
            this.g0i = ((s2g * l) / s3g);
            this.g1s = (s1g / ((s3g + 126452)));
            this.g1i = ((((s2g - 769860)) * l) / ((s3g + 126452)));
            this.b0s = (s1b / s3b);
            this.b0i = ((s2b * l) / s3b);
            this.b1s = (s1b / ((s3b + 126452)));
            this.b1i = ((((s2b - 769860)) * l) / ((s3b + 126452)));
        }


        public virtual double calcMaxChromaHpluv()
        {
            double r0 = HsluvConverterFast.distanceFromOrigin(this.r0s, this.r0i);
            double r1 = HsluvConverterFast.distanceFromOrigin(this.r1s, this.r1i);
            double g0 = HsluvConverterFast.distanceFromOrigin(this.g0s, this.g0i);
            double g1 = HsluvConverterFast.distanceFromOrigin(this.g1s, this.g1i);
            double b0 = HsluvConverterFast.distanceFromOrigin(this.b0s, this.b0i);
            double b1 = HsluvConverterFast.distanceFromOrigin(this.b1s, this.b1i);
            return HsluvConverterFast.min6(r0, r1, g0, g1, b0, b1);
        }


        public virtual double calcMaxChromaHsluv(double h)
        {
            double hueRad = (((h / 360) * Math.PI) * 2);
            double r0 = HsluvConverterFast.distanceFromOriginAngle(this.r0s, this.r0i, hueRad);
            double r1 = HsluvConverterFast.distanceFromOriginAngle(this.r1s, this.r1i, hueRad);
            double g0 = HsluvConverterFast.distanceFromOriginAngle(this.g0s, this.g0i, hueRad);
            double g1 = HsluvConverterFast.distanceFromOriginAngle(this.g1s, this.g1i, hueRad);
            double b0 = HsluvConverterFast.distanceFromOriginAngle(this.b0s, this.b0i, hueRad);
            double b1 = HsluvConverterFast.distanceFromOriginAngle(this.b1s, this.b1i, hueRad);
            return HsluvConverterFast.min6(r0, r1, g0, g1, b0, b1);
        }


        public virtual void hsluvToLch()
        {
            if ((this.hsluv_l > 99.9999999))
            {
                this.lch_l = ((100));
                this.lch_c = ((0));
            }
            else if ((this.hsluv_l < 0.00000001))
            {
                this.lch_l = ((0));
                this.lch_c = ((0));
            }
            else
            {
                this.lch_l = this.hsluv_l;
                this.calculateBoundingLines(this.hsluv_l);
                double max = this.calcMaxChromaHsluv(this.hsluv_h);
                this.lch_c = ((max / 100) * this.hsluv_s);
            }

            this.lch_h = this.hsluv_h;
        }


        public virtual void lchToHsluv()
        {
            if ((this.lch_l > 99.9999999))
            {
                this.hsluv_s = ((0));
                this.hsluv_l = ((100));
            }
            else if ((this.lch_l < 0.00000001))
            {
                this.hsluv_s = ((0));
                this.hsluv_l = ((0));
            }
            else
            {
                this.calculateBoundingLines(this.lch_l);
                double max = this.calcMaxChromaHsluv(this.lch_h);
                this.hsluv_s = ((this.lch_c / max) * 100);
                this.hsluv_l = this.lch_l;
            }

            this.hsluv_h = this.lch_h;
        }


        public virtual void hpluvToLch()
        {
            if ((this.hpluv_l > 99.9999999))
            {
                this.lch_l = ((100));
                this.lch_c = ((0));
            }
            else if ((this.hpluv_l < 0.00000001))
            {
                this.lch_l = ((0));
                this.lch_c = ((0));
            }
            else
            {
                this.lch_l = this.hpluv_l;
                this.calculateBoundingLines(this.hpluv_l);
                double max = this.calcMaxChromaHpluv();
                this.lch_c = ((max / 100) * this.hpluv_p);
            }

            this.lch_h = this.hpluv_h;
        }


        public virtual void lchToHpluv()
        {
            if ((this.lch_l > 99.9999999))
            {
                this.hpluv_p = ((0));
                this.hpluv_l = ((100));
            }
            else if ((this.lch_l < 0.00000001))
            {
                this.hpluv_p = ((0));
                this.hpluv_l = ((0));
            }
            else
            {
                this.calculateBoundingLines(this.lch_l);
                double max = this.calcMaxChromaHpluv();
                this.hpluv_p = ((this.lch_c / max) * 100);
                this.hpluv_l = this.lch_l;
            }

            this.hpluv_h = this.lch_h;
        }


        public virtual void hsluvToRgb()
        {
            this.hsluvToLch();
            this.lchToLuv();
            this.luvToXyz();
            this.xyzToRgb();
        }


        public virtual void hpluvToRgb()
        {
            this.hpluvToLch();
            this.lchToLuv();
            this.luvToXyz();
            this.xyzToRgb();
        }


        public virtual void hsluvToHex()
        {
            this.hsluvToRgb();
            this.rgbToHex();
        }


        public virtual void hpluvToHex()
        {
            this.hpluvToRgb();
            this.rgbToHex();
        }


        public virtual void rgbToHsluv()
        {
            this.rgbToXyz();
            this.xyzToLuv();
            this.luvToLch();
            this.lchToHpluv();
            this.lchToHsluv();
        }


        public virtual void rgbToHpluv()
        {
            this.rgbToXyz();
            this.xyzToLuv();
            this.luvToLch();
            this.lchToHpluv();
            this.lchToHpluv();
        }


        public virtual void hexToHsluv()
        {
            this.hexToRgb();
            this.rgbToHsluv();
        }


        public virtual void hexToHpluv()
        {
            this.hexToRgb();
            this.rgbToHpluv();
        }
    }
}

