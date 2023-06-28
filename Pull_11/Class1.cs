using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Pull_11
{

    //Вытяжка - Витя Ф
    public class Pulling
    {
        private double _diameterInitial;
        private double _diameterFinished;
        private double _thicknessInitial;
        private double _thicknessFinished;
        private double _coefficientOfStock;
        private double _utmostCoefficientPulling_FirstOperation;
        private double _utmostCoefficientThinning_FirstOperation;
        private double _utmostCoefficientPulling_SubsequentOperation;
        private double _utmostCoefficientThinning_SubsequentOperation;
        private double _wallThicknessFinDetail_UpperSection;
        private double _wallThicknessFinDetail_LowerSection;
        private double _volumeFinishedProduct; // объём готового издели
        private double _ultimateStrength; // Предел прочности
        public double _amountPulling;

        public List<double> AssignmentPullingCoefficient = new List<double>();
        public List<double> AssignmentThinningCoefficient = new List<double>();
        public List<double> CoefficientPull = new List<double>();
        public List<double> CoefficientThinn = new List<double>();
        public List<double> MedianDiameterSFP = new List<double>();
        public List<double> WallThicknessUS = new List<double>();
        public List<double> WallThicknessLS = new List<double>();
        public List<double> OutsideDiameter = new List<double>();
        public List<double> HeightSFP = new List<double>();
        public List<double> InsideDiameterUS = new List<double>();
        public List<double> InsideDiameterLS = new List<double>();
        public List<double> PairingRad = new List<double>();
        public List<double> DistanceBottomUS = new List<double>();
        public List<double> ElasticDeformationMatrix = new List<double>();
        public List<double> ElasticUnloading = new List<double>();
        public List<double> TotalElastic_DeformUnload = new List<double>();
        public List<double> ExecutiveDimensionsMatr = new List<double>();
        public List<double> MinInsideDiameter_US = new List<double>();
        public List<double> MinInsideDiameter_LS = new List<double>();
        public List<double> ThermalDeformatiomHobUS = new List<double>();
        public List<double> ThermalDeformatiomHobLS = new List<double>();
        public List<double> ExecutiveDimensionsHobUS = new List<double>();
        public List<double> ExecutiveDimensionsHobLS = new List<double>();
        public List<double> CoefficientThinningLS = new List<double>();
        public List<double> DegreeDeformationUS = new List<double>();
        public List<double> DegreeDeformationLS = new List<double>();



        private static double Kvalitet6(double DiameterInitial)
        {
            double Admittance = 0;
            if (DiameterInitial <= 3)
            {
                Admittance = -0.006;
            }
            if (3 < DiameterInitial && DiameterInitial <= 6)
            {
                Admittance = -0.008;
            }
            if (6 < DiameterInitial && DiameterInitial <= 10)
            {
                Admittance = -0.009;
            }
            if (10 < DiameterInitial && DiameterInitial <= 18)
            {
                Admittance = -0.011;
            }
            if (18 < DiameterInitial && DiameterInitial <= 30)
            {
                Admittance = -0.013;
            }
            if (30 < DiameterInitial && DiameterInitial <= 50)
            {
                Admittance = -0.016;
            }
            if (50 < DiameterInitial && DiameterInitial <= 80)
            {
                Admittance = -0.019;
            }
            if (80 < DiameterInitial && DiameterInitial <= 120)
            {
                Admittance = -0.022;
            }
            if (120 < DiameterInitial && DiameterInitial <= 180)
            {
                Admittance = -0.025;
            }
            if (180 < DiameterInitial && DiameterInitial <= 250)
            {
                Admittance = -0.029;
            }
            if (250 < DiameterInitial && DiameterInitial <= 315)
            {
                Admittance = -0.032;
            }
            if (315 < DiameterInitial && DiameterInitial <= 400)
            {
                Admittance = -0.036;
            }
            if (400 < DiameterInitial && DiameterInitial <= 500)
            {
                Admittance = -0.04;
            }
            double result = Admittance;
            return result;
        }



        public Pulling(double DiameterInitial, double DiameterFinished, double ThicknessInitial, double ThicknessFinished, double CoefficientOfStock,
                       double UtmostCoefficientPulling_FirstOperation, double UtmostCoefficientThinning_FirstOperation,
                       double UtmostCoefficientPulling_SubsequentOperation, double UtmostCoefficientThinning_SubsequentOperation,
                       double WallThicknessFinDetail_UpperSection, double WallThicknessFinDetail_LowerSection, double VolumeFinishedProduct, double ultimateStrength)
        {
            _diameterInitial = DiameterInitial;
            _diameterFinished = DiameterFinished;
            _thicknessInitial = ThicknessInitial;
            _thicknessFinished = ThicknessFinished;
            _coefficientOfStock = CoefficientOfStock;
            _utmostCoefficientPulling_FirstOperation = UtmostCoefficientPulling_FirstOperation;
            _utmostCoefficientThinning_FirstOperation = UtmostCoefficientThinning_FirstOperation;
            _utmostCoefficientPulling_SubsequentOperation = UtmostCoefficientPulling_SubsequentOperation;
            _utmostCoefficientThinning_SubsequentOperation = UtmostCoefficientThinning_SubsequentOperation;
            _wallThicknessFinDetail_UpperSection = WallThicknessFinDetail_UpperSection;
            _wallThicknessFinDetail_LowerSection = WallThicknessFinDetail_LowerSection;
            _volumeFinishedProduct = VolumeFinishedProduct;
            _ultimateStrength = ultimateStrength;
        }
        public void CalculationPulling()
        {
            //Суммарный коэффициент вытяжки
            double TotalCoefficientPulling = _diameterFinished / _diameterInitial;
            TotalCoefficientPulling = Math.Round(TotalCoefficientPulling, 2);
            //Суммарный коэффициент утонения
            double TotalCoefficientThinning = _thicknessFinished / _thicknessInitial;
            TotalCoefficientThinning = Math.Round(TotalCoefficientThinning, 2);


            // Расчет количества операций
            double ManufacturingCondition_FirstOperation_Pulling = _coefficientOfStock * _utmostCoefficientPulling_FirstOperation;
            double ManufacturingCondition_FirstOperation_Thinning = _coefficientOfStock * _utmostCoefficientThinning_FirstOperation;
            ManufacturingCondition_FirstOperation_Pulling = Math.Round(ManufacturingCondition_FirstOperation_Pulling, 2);
            ManufacturingCondition_FirstOperation_Thinning = Math.Round(ManufacturingCondition_FirstOperation_Thinning, 2);

            double AmountPulling = 1;
            double CoefficientConvolution_Pulling = 1;
            double CoefficientConvolution_Thinning = 1;
            double ManufacturingCondition_SubsequentOperation_Pulling = 1;
            double ManufacturingCondition_SubsequentOperation_Thinning = 1;

            double CoefficientOfPulling;
            double CoefficientOfThinning;

            double CoefficientPulling;
            double CoefficientThinning;

            if (TotalCoefficientPulling >= ManufacturingCondition_FirstOperation_Pulling & TotalCoefficientThinning >= ManufacturingCondition_FirstOperation_Thinning)
            {
                _utmostCoefficientPulling_FirstOperation = TotalCoefficientPulling;
                _utmostCoefficientThinning_FirstOperation = TotalCoefficientPulling;
            }


            else
            {
                CoefficientConvolution_Pulling = ManufacturingCondition_FirstOperation_Pulling;
                CoefficientConvolution_Thinning = ManufacturingCondition_FirstOperation_Thinning;
                ManufacturingCondition_SubsequentOperation_Pulling = _coefficientOfStock * _utmostCoefficientPulling_SubsequentOperation;
                ManufacturingCondition_SubsequentOperation_Thinning = _coefficientOfStock * _utmostCoefficientThinning_SubsequentOperation;
                Math.Round(ManufacturingCondition_SubsequentOperation_Pulling, 2);
                Math.Round(ManufacturingCondition_SubsequentOperation_Thinning, 2);
                CoefficientPulling = TotalCoefficientPulling / CoefficientConvolution_Pulling;
                CoefficientThinning = TotalCoefficientThinning / CoefficientConvolution_Thinning;

                do
                {
                    CoefficientOfPulling = Math.Pow(CoefficientPulling, (1 / AmountPulling));
                    CoefficientOfThinning = Math.Pow(CoefficientThinning, (1 / AmountPulling));
                    Math.Round(CoefficientOfPulling, 2);
                    Math.Round(CoefficientOfThinning, 2);

                    AmountPulling += 1;
                } while (CoefficientOfPulling < ManufacturingCondition_SubsequentOperation_Pulling || CoefficientOfThinning < ManufacturingCondition_SubsequentOperation_Thinning);

            }
            _amountPulling = AmountPulling;
            //Назначение коэффициентов вытяжки по операциям

            double k = 1; double CoeffPull = 1;


            double PullingComposition(double j)
            {
                CoefficientPull.Clear();
                for (int i = 1; i <= _amountPulling; i++)
                {

                    // Console.Write("Введите значение коэффициента вытяжки для " + i + " операции: " + " ");
                    CoeffPull = double.Parse(Console.ReadLine()); // Не хочет переводить string в double гадюка 
                    CoefficientPull.Add(CoeffPull);
                    j *= CoeffPull;
                    j = Math.Round(j, 3);
                }
                return j;
            }

            int count_k = 1;

            while (TotalCoefficientPulling != k)
            {
                if (count_k > 1)
                    Console.WriteLine("Условие" + TotalCoefficientPulling + "=" + k + " не выполняется. Введите значение ещё раз.");

                k = PullingComposition(1);
                count_k++;
            }

            //Назначение коэффициентов утонения по операциям

            double kk = 1; double CoeffThin = 1;

            double ThinningComposition(double j)
            {
                CoefficientThinn.Clear();
                for (int i = 1; i <= _amountPulling; i++)
                {

                    //Console.Write("Введите значение коэффициента вытяжки для " + i + " операции: " + " ");
                    CoeffThin = double.Parse(Console.ReadLine());
                    CoefficientThinn.Add(CoeffThin);
                    j *= CoeffThin;
                    j = Math.Round(j, 3);// было 3
                }
                return j;
            }

            int count_j = 1;

            while (TotalCoefficientThinning != kk)
            {
                if (count_j > 1)
                    Console.WriteLine("Условие" + TotalCoefficientThinning + "=" + k + " не выполняется. Введите значение ещё раз.");

                kk = ThinningComposition(1);
                count_j++;
            }

            //Срединный диаметр полкуфабриката

            int c; double MedianDiameterSFPr = 1; double MedianDiameter = _diameterInitial; // SFPr (SFP) - semi finished product

            for (c = 0; _amountPulling > 0; c++)
            {
                MedianDiameterSFPr = MedianDiameter * CoefficientPull[c];
                MedianDiameterSFPr = Math.Round(MedianDiameterSFPr, 2);
                MedianDiameterSFP.Add(MedianDiameterSFPr);
                _amountPulling -= 1;
                MedianDiameter = MedianDiameterSFPr;
            }
            _amountPulling = AmountPulling;

            //Толщина стенки в верхнем расчетном сечении

            double WallThickness_UpperSection = 1; double WallThickness_US = _thicknessInitial;

            for (c = 0; _amountPulling > 0; c++)
            {

                WallThickness_UpperSection = WallThickness_US * CoefficientPull[c];
                WallThickness_UpperSection = Math.Round(WallThickness_UpperSection, 2);
                WallThicknessUS.Add(WallThickness_UpperSection);
                _amountPulling -= 1;
                WallThickness_US = WallThickness_UpperSection;
            }
            _amountPulling = AmountPulling;
            //Толщина стенки в нижнем расчетном сечении


            double WallThickness_LowerSection = 1; double WallThickness_LS = _thicknessInitial;

            for (c = 0; _amountPulling > 0; c++)
            {

                WallThickness_LowerSection = WallThicknessUS[c] + (_wallThicknessFinDetail_LowerSection - _wallThicknessFinDetail_UpperSection);
                WallThickness_LowerSection = Math.Round(WallThickness_LowerSection, 2);
                WallThicknessLS.Add(WallThickness_LowerSection);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            //Наружный диаметр полуфабриката

            double OutsideDiameterSFPr = 1;

            for (c = 0; _amountPulling > 0; c++)
            {
                OutsideDiameterSFPr = WallThicknessUS[c] + MedianDiameterSFP[c];
                OutsideDiameterSFPr = Math.Round(OutsideDiameterSFPr, 2);
                OutsideDiameter.Add(OutsideDiameterSFPr);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            // Высота полуфабриаката по операциям

            double HeightSFPr = 1;

            for (c = 0; _amountPulling > 0; c++)
            {
                HeightSFPr = (((Math.Pow(_diameterInitial, 2) - Math.Pow(OutsideDiameter[c], 2)) * _thicknessInitial) / (4 * MedianDiameterSFP[c] * WallThicknessUS[c])) + _thicknessInitial;
                HeightSFPr = Math.Round(HeightSFPr, 2);
                HeightSFP.Add(HeightSFPr);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            //Внутрений диаметр в верхнем расчетном сечении

            double InsideDiameter_UpperSection = 1;

            for (c = 0; _amountPulling > 0; c++)
            {
                InsideDiameter_UpperSection = OutsideDiameter[c] - (2 * WallThicknessUS[c]);
                InsideDiameter_UpperSection = Math.Round(InsideDiameter_UpperSection, 2);
                InsideDiameterUS.Add(InsideDiameter_UpperSection);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            //Внутрений диаметр в нижнем расчетном сечении

            double InsideDiameter_LowerSection = 1;

            for (c = 0; _amountPulling > 0; c++)
            {
                InsideDiameter_LowerSection = OutsideDiameter[c] - (2 * WallThicknessLS[c]);
                InsideDiameter_LowerSection = Math.Round(InsideDiameter_LowerSection, 2);
                InsideDiameterLS.Add(InsideDiameter_LowerSection);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            //Радиус сопряжения

            double PairingRadius = 1;

            for (c = 0; _amountPulling > 0; c++)
            {
                PairingRadius = 0.11 * OutsideDiameter[c];
                PairingRadius = Math.Round(PairingRadius, 2);
                PairingRad.Add(PairingRadius);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            //Расстояние от дна до верхнего расчетного сечения

            double DistanceBottom_from_UpperSection = 1;

            for (c = 0; _amountPulling > 0; c++)
            {
                DistanceBottom_from_UpperSection = HeightSFP[c] - _thicknessInitial - ((0.15 * _volumeFinishedProduct) / (Math.PI * MedianDiameterSFP[c] * WallThicknessUS[c]));
                DistanceBottom_from_UpperSection = Math.Round(DistanceBottom_from_UpperSection, 2);
                DistanceBottomUS.Add(DistanceBottom_from_UpperSection);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;



            /* Это закоментил пока просто мб всё же понадобится. Расчет расчет исполнительных размеров инструментов.
            //Упругая деформация матрицы

            double ElasticDeformationOfTheMatrix = 1;

            for (c = 0; _amountPulling > 0; c++)
            {
                ElasticDeformationOfTheMatrix = 0.02 * OutsideDiameter[c];
                ElasticDeformationOfTheMatrix = Math.Round(ElasticDeformationOfTheMatrix, 2);
                ElasticDeformationMatrix.Add(ElasticDeformationOfTheMatrix);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            //Упругая разгрузка матрицы

            double ElasticUnloadingSFPr = 1;


            for (c = 0; _amountPulling > 0; c++)
            {
                ElasticUnloadingSFPr = 0.006 * ((WallThicknessUS[c] + WallThicknessLS[c]) / 2);
                ElasticUnloadingSFPr = Math.Round(ElasticUnloadingSFPr, 2);
                ElasticUnloading.Add(ElasticUnloadingSFPr);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            //Суммарная упругая деформация и разгрузка матрицы

            double TotalElastic_Deform_Unload = 1;
            

            for (c = 0; _amountPulling > 0; c++)
            {
                TotalElastic_Deform_Unload = ElasticDeformationMatrix[c] + ElasticUnloading[c];
                TotalElastic_Deform_Unload = Math.Round(TotalElastic_Deform_Unload, 2);
                TotalElastic_DeformUnload.Add(TotalElastic_Deform_Unload);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;

            //Исполнительные размеры матрицы

            double ExecutiveDimensionsMatrix = 1;
            

            for (c = 0; AmountPulling > 0; c++)
            {
                double Admittance = Kvalitet6(OutsideDiameter[c]);
                double Tdn = Admittance;
                ExecutiveDimensionsMatrix = OutsideDiameter[c] - TotalElastic_DeformUnload[c] - Tdn;
                ExecutiveDimensionsMatrix = Math.Round(ExecutiveDimensionsMatrix, 2);
                ExecutiveDimensionsMatr.Add(ExecutiveDimensionsMatrix);
                AmountPulling -= 1;
            }
            _amountPulling = AmountPulling;

            //Исполнительные размеры пуансона
            //Минимальный внутренний диаметр заготовки в верхнем расчетном сечении

            double MinInsideDiameter_UpperSection = 1;
            

            for (c = 0; _amountPulling > 0; c++)
            {

                MinInsideDiameter_UpperSection = InsideDiameterUS[c] + 0;
                MinInsideDiameter_UpperSection = Math.Round(MinInsideDiameter_UpperSection, 2);
                MinInsideDiameter_US.Add(MinInsideDiameter_UpperSection);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;

            //Минимальный внутренний диаметр заготовки в нмижнем расчетном сечении

            double MinInsideDiameter_LowerSection = 1;
            

            for (c = 0; _amountPulling > 0; c++)
            {

                MinInsideDiameter_LowerSection = InsideDiameterLS[c] + 0;
                MinInsideDiameter_LowerSection = Math.Round(MinInsideDiameter_LowerSection, 2);
                MinInsideDiameter_LS.Add(MinInsideDiameter_LowerSection);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            //Тепловая деформация пуансона в верхнем расчетном сечении

            double ThermalDeformatiomHob_UpperSection = 1;
            

            for (c = 0; _amountPulling > 0; c++)
            {
                ThermalDeformatiomHob_UpperSection = InsideDiameterUS[c] * 0.01;
                ThermalDeformatiomHob_UpperSection = Math.Round(ThermalDeformatiomHob_UpperSection, 2);
                ThermalDeformatiomHobUS.Add(ThermalDeformatiomHob_UpperSection);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;

            //Тепловая деформация пуансона в нижнем расчетном сечении

            double ThermalDeformatiomHob_LowerSection = 1;
            

            for (c = 0; _amountPulling > 0; c++)
            {
                ThermalDeformatiomHob_LowerSection = InsideDiameterLS[c] * 0.01;
                ThermalDeformatiomHob_LowerSection = Math.Round(ThermalDeformatiomHob_LowerSection, 2);
                ThermalDeformatiomHobLS.Add(ThermalDeformatiomHob_LowerSection);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;

            //Исполнительные размер пуансона в верхнем расчетном сечении

            double ExecutiveDimensionsHob_UpperSection = 1;
            

            for (c = 0; _amountPulling > 0; c++)
            {
                double Admittance = Kvalitet6(MinInsideDiameter_US[c]);
                double Tdn = Admittance;
                double PP = -Tdn * 0.5;
                ExecutiveDimensionsHob_UpperSection = MinInsideDiameter_US[c] + PP - ThermalDeformatiomHobUS[c] + Tdn;
                ExecutiveDimensionsHob_UpperSection = Math.Round(ExecutiveDimensionsHob_UpperSection, 2);
                ExecutiveDimensionsHobUS.Add(ExecutiveDimensionsHob_UpperSection);
                _amountPulling -= 1;
            }

            _amountPulling = AmountPulling;
            //Исполнительные размерs пуансона в нижнем расчетном сечении

            double ExecutiveDimensionsHob_LowerSection = 1;
            

            for (c = 0; _amountPulling > 0; c++)
            {
                double Admittance = Kvalitet6(MinInsideDiameter_LS[c]);
                double Tdn = Admittance;
                double PP = -Tdn * 0.5;
                ExecutiveDimensionsHob_LowerSection = MinInsideDiameter_LS[c] + PP - ThermalDeformatiomHobUS[c] + Tdn;
                ExecutiveDimensionsHob_LowerSection = Math.Round(ExecutiveDimensionsHob_LowerSection, 2);
                ExecutiveDimensionsHobLS.Add(ExecutiveDimensionsHob_LowerSection);
                _amountPulling -= 1;
            }
            _amountPulling = AmountPulling;
            //Коэффициент утонения в нижнем расчетном сечении

            double CoefficientThinning_LowerSection = 1; double ThicknessInitial0 = _thicknessInitial;
            

            for (c = 0; _amountPulling > 0; c++)
            {
                CoefficientThinning_LowerSection = WallThicknessLS[c] / ThicknessInitial0;
                CoefficientThinning_LowerSection = Math.Round(CoefficientThinning_LowerSection, 2);
                CoefficientThinningLS.Add(CoefficientThinning_LowerSection);
                ThicknessInitial0 = CoefficientThinning_LowerSection;
                _amountPulling -= 1;
            }

            _amountPulling = AmountPulling;
            //Степень деформации в верхнем расчетном сечении

            double DegreeDeformation_UpperSection = 1;
            

            for (c = 0; _amountPulling > 0; c++)
            {
                DegreeDeformation_UpperSection = (2 / Math.Pow(3, 0.5)) * Math.Log((1 / CoefficientThinn[c] + CoefficientPull[c]));
                DegreeDeformation_UpperSection = Math.Round(DegreeDeformation_UpperSection, 2);
                DegreeDeformationUS.Add(DegreeDeformation_UpperSection);
                _amountPulling -= 1;
            }

            _amountPulling = AmountPulling;
            //Степень деформации в нижнем расчетном сечении

            double DegreeDeformation_LowerSection = 1;
            

            for (c = 0; _amountPulling > 0; c++)
            {
                DegreeDeformation_LowerSection = (2 / Math.Pow(3, 0.5)) * Math.Log((1 / CoefficientThinningLS[c] + CoefficientPull[c]));
                DegreeDeformation_LowerSection = Math.Round(DegreeDeformation_LowerSection, 2);
                DegreeDeformationLS.Add(DegreeDeformation_LowerSection);
                _amountPulling -= 1;
            }
            */

            ВырСвертка(_thicknessInitial, CoefficientPull[0], 1, _diameterInitial, MedianDiameterSFP[0], PairingRad[0],
                       _thicknessInitial, _ultimateStrength, CoefficientThinn[0], WallThicknessUS[0]);
        }

        public static void ВырСвертка(double sd, double md1, double psir, double d0, double d1, double rp,
                                      double s, double sigmb, double ms1, double s1)
        {

            Console.WriteLine("Введите тип матрицы: матрица конич.-K, радиальная-R"); // 2 Типа матриц
            string tipMatriz = Console.ReadLine();
            /*double sd = double.Parse(Console.ReadLine()); // Толщина дна
            double md1 = double.Parse(Console.ReadLine()); // Коеф свертки
            double psir = double.Parse(Console.ReadLine()); // Пси р по материалу*/

            // Проверка на прижим
            if (sd > (1 - md1) / 18)
            {
                Console.WriteLine("прижим не нужен на кон. и рад. матр");
            }
            else if (sd > (1 - md1) / 36)
            {
                Console.WriteLine("на конич. матр. прижим не нужен");
            }
            else
            {
                Console.WriteLine("нужен прижим");
            }

            // Разветвление конической
            if (tipMatriz == "k")
            {
                if (sd > 0.012 && sd < 0.05) // толщина 0.012 - 0.05 мм Серьёзно?
                {
                    Console.WriteLine("Рекомендуется двухконусная матрица");
                }
                else if (sd > 0.05)
                {
                    Console.WriteLine("Рекомендуется одноконусная матрица с большим радиусом");
                }

                // Проверка конической на прижим
                if (sd <= (1 - md1) / 36)
                {
                    //Console.WriteLine("одноконМатрПрижим");
                    double pi = Math.PI;
                    /* double d0 = _thicknessInitial;
                     double d1 = double.Parse(Console.ReadLine());// диаметр после 1-ой вытяжки наверное (пускай срединный будет)
                     double rp = double.Parse(Console.ReadLine());//  радиус пуансона*/
                    //double s = double.Parse(Console.ReadLine());// толщина 
                    double rps = rp + s / 2;
                    //double sigmb = double.Parse(Console.ReadLine());// предел прочности материала
                    double dm1 = double.Parse(Console.ReadLine());// диаметр матрицы первой
                    double alf = double.Parse(Console.ReadLine());// угол конусности
                    double al = pi * alf / 180;// это типо перевод из градусов в радианы
                    //double ms1 = double.Parse(Console.ReadLine());// утонение на 1-ой операции
                    //double s1 = double.Parse(Console.ReadLine());// толщина стенки на первой операции?
                    double mum = 0.05;

                    double dk = d1 * Math.Sqrt((1 / md1 * md1 - 1 - 2.28 * rps / d1 + 0.07 * alf * rps / d1 + 0.56 * (rps / d1) * (rps / d1)) * Math.Sin(pi * alf / 180) + 1);
                    dk = Math.Round(dk, 1);

                    double hk = (dk - dm1) / 2 / Math.Tan(al);
                    hk = Math.Round(hk, 1);

                    double rw = 3 * s;
                    double rws = rw + s / 2;

                    double md12 = d1 / dk;
                    double md11 = dk / d0;
                    double psi = 1 - dk / d0;
                    double fik = pi * (90 - alf) / 180;
                    double psisr = 1 - Math.Sqrt(md1);
                    double sigms = sigmb * Math.Pow((psisr / psir), psir / (1 - psir)) / (1 - psir);
                    sigms = Math.Round(sigms, 1);

                    double a = Math.Log(1 / md11) - psi + s / (2 * rws * Math.Sqrt(md11));
                    double b = 1 + mum * fik;
                    double c = 1 - 18 * sd / (1 - md1);
                    double sigmr = 1.1 * sigms * (a * b / (1 - 0.2 * mum * b * c / md1) + Math.Log(1 / md12));
                    sigmr = Math.Round(sigmr, 1);

                    if (ms1 == 1)
                    {
                        //усилие при свертке без утонения
                        double pbu = Math.PI * d1 * s * sigmr;
                        pbu = Math.Round(pbu, 0);
                        //Console.WriteLine("sigms = " + sigms + " kg / mm^2");
                        //Console.WriteLine("sigmr = " + sigmr + " kg / mm^2");
                        //Console.WriteLine("усилие свертки без утонения=" + pbu + " кг");
                    }
                    else
                    {
                        double sigms2 = sigmas2(sigmb, md1, ms1, psir);
                        sigms2 = Math.Round(sigms2, 1);
                        double sigmz = sigmaz(mum, sigmr, sigms2, al, ms1);
                        sigmz = Math.Round(sigmz, 1);
                        //усилие при свертке с утонением
                        double pu = Math.PI * d1 * s1 * sigmz;
                        pu = Math.Round(pu, 0);
                        //конечная стадия
                        double msk = ms1 * Math.Sqrt(md1);
                        double sigmrk = 0;
                        double sigms2k = sigmas2(sigmb, md1, msk, psir);
                        sigms2k = Math.Round(sigms2k, 1);
                        double sigmzk = sigmazk(mum, msk, al, sigms2k); ;
                        sigmzk = Math.Round(sigmzk, 1);
                        //усилие при свертке с утонением в конечн. стадии
                        double pk = Math.PI * d1 * s * sigmzk;
                        pk = Math.Round(pk, 0);

                        //Console.WriteLine("sigms = " + sigms2 + " kg / mm^2");
                        //Console.WriteLine("sigmzk = " + sigmz + " kg / mm^2");
                        //Console.WriteLine("усилие свертки =" + pu + " кг");
                        //Console.WriteLine("конечная стадия");
                        //Console.WriteLine("sigms = " + sigms2k + " kg / mm^2");
                        //Console.WriteLine("sigmzk = " + sigmzk + " kg / mm^2");
                        //Console.WriteLine("усилие свертки =" + pk + " кг");
                    }
                    double ustKonus = (Math.Sqrt((20 * sd) * (20 * sd) * (1 - Math.Sin(al)) + Math.Sin(al) * Math.Sin(al)) - 20 * sd) / Math.Sin(al);
                    //проверка на склакообразованиие в конусе матрицы
                    if (md1 < ustKonus)
                    {
                        //Console.WriteLine("Однокон. матр.с плоск.и КОНИЧ. прижимом");
                    }
                    else
                    {
                        //Console.WriteLine("Одноконус. матрица с прижимом");
                    }
                    //Console.WriteLine("Диаметр входн.кромки конуса = " + dk);
                    //Console.WriteLine("Радиус входн.кромки конуса = " + rw);
                    //Console.WriteLine("Высота конуса = " + hk);
                    //Console.WriteLine("Угол конуса = " + alf + " град");
                }
                else
                {
                    // тут должен быть ввод матрицы с интерфейса
                    Console.WriteLine("матрица однокон.-O, двухкон.-D, однокон. с радиус-OR");
                    string tipKonMatriz = Console.ReadLine();

                    if (tipKonMatriz == "o")
                    {
                        // Опять же ввод перестыковать
                        //Console.WriteLine("одноконМатрБезПриж");
                        double pi = Math.PI;
                        /* double d0 = double.Parse(Console.ReadLine()); // Диаметр кружка
                         double d1 = double.Parse(Console.ReadLine()); // Диаметр после свертки ?
                         double rp = double.Parse(Console.ReadLine()); // Радиус пуансона */
                        //double s = double.Parse(Console.ReadLine()); // Толщина
                        double rps = rp + s / 2;
                        //double sigmb = double.Parse(Console.ReadLine());
                        double dm1 = double.Parse(Console.ReadLine()); // Диаметр верхней матрицы
                        //Console.WriteLine("Введи угол конусности матрицы");
                        double alf = double.Parse(Console.ReadLine()); // Угол конуса
                        double al = pi * alf / 180;
                        //double ms1 = double.Parse(Console.ReadLine()); // Коеф
                        //double s1 = double.Parse(Console.ReadLine()); // Толщина после свертки? поставил толщину вверху (есть также внизу)
                        double mum = 0.05;

                        double dk = Math.Round(0.9 * d0, 1);
                        double hk = Math.Round((dk - dm1) / 2 / Math.Tan(al), 1);
                        double rw = Math.Round(0.05 * d0, 1);
                        double rws = rw + s / 2;
                        double dkk = d1 * Math.Sqrt((1 / md1 * md1 - 1 - 2.28 * rps / d1 + 0.07 * alf * rps / d1 + 0.56 * (rps / d1) * (rps / d1)) * Math.Sin(pi * alf / 180) + 1);

                        double md12 = d1 / dkk;
                        double md11 = dkk / d0;
                        double psi = 1 - dkk / d0;
                        double psisr = 1 - Math.Sqrt(md1);
                        double sigms = sigmb * Math.Pow((psisr / psir), (psir / (1 - psir))) / (1 - psir);
                        sigms = Math.Round(sigms, 1);
                        double sigmr = 1.1 * sigms * (1 + mum / Math.Tan(al)) * Math.Log(1 / md12);
                        sigmr = Math.Round(sigmr, 1);
                        if (ms1 == 1)
                        {
                            //усилие при свертке без утонения
                            double pbu = pi * d1 * s * sigmr;
                            pbu = Math.Round(pbu, 0);
                            //Console.WriteLine("sigms={0} кг / мм^2", sigms);
                            //Console.WriteLine("sigmr={0} кг / мм^2", sigmr);
                            //Console.WriteLine("усилие свертки={0} кг", pbu);
                        }
                        else
                        {
                            double sigms2 = sigmas2(sigmb, md1, ms1, psir);
                            sigms = Math.Round(sigms2, 1);
                            double sigmz = sigmaz(mum, sigmr, sigms2, al, ms1);
                            sigmz = Math.Round(sigmz, 1);
                            //усилие при свертке с утонением
                            double pu = pi * d1 * s1 * sigmz;
                            pu = Math.Round(pu, 0);
                            //конечная стадия
                            double msk = ms1 * Math.Sqrt(md1);
                            double sigmrk = 0;
                            double sigms2k = sigmas2(sigmb, md1, msk, psir);
                            sigms2k = Math.Round(sigms2k, 1);
                            double sigmzk = sigmazk(mum, msk, al, sigms2k);
                            sigmzk = Math.Round(sigmzk, 1);
                            //усилие при свертке с утонением в конечн. стадии
                            double pk = pi * d1 * s1 * sigmzk;
                            pk = Math.Round(pk, 0);
                            //Console.WriteLine("sigms={0} кг / мм^2", sigms2);
                            //Console.WriteLine("sigmr={0} кг / мм^2", sigmz);
                            //Console.WriteLine("усилие свертки={0} кг", pu);
                            //Console.WriteLine("конечная стадия");
                            //Console.WriteLine("sigms={0} кг / мм^2", sigms2k);
                            //Console.WriteLine("sigmr={0} кг / мм^2", sigmzk);
                            //Console.WriteLine("усилие свертки={0} кг", pk);
                        }
                        //Console.WriteLine("Одноконус. матрица без прижима");
                        //Console.WriteLine("Диаметр входн.кромки конуса={0}", dk);
                        //Console.WriteLine("Радиус входн.кромки конуса={0}", rw);
                        //Console.WriteLine("Высота конуса={0}", hk);
                        //Console.WriteLine("Угол конуса={0}", alf);
                    }
                    else if (tipKonMatriz == "d")
                    {
                        //Console.WriteLine("двухконМатр");
                        // Define constants
                        double mum = 0.05;
                        double Pi = Math.PI;

                        // Такой же ввод
                        /* double d0 = double.Parse(Console.ReadLine());
                         double d1 = double.Parse(Console.ReadLine());*/
                        double dm1 = double.Parse(Console.ReadLine());
                        //double rp = double.Parse(Console.ReadLine());
                        //double s = double.Parse(Console.ReadLine());
                        //double ms1 = double.Parse(Console.ReadLine());
                        //double s1 = double.Parse(Console.ReadLine());
                        double rps = rp + s / 2;
                        //double sigmb = double.Parse(Console.ReadLine());


                        // Calculate recommended angle for upper cone
                        if (sd > 0.012 && sd < 0.018)
                        {
                            string werhugol = "30 degrees";
                            //Console.WriteLine("Recommended angle for upper cone = " + werhugol);
                        }
                        else if (sd > 0.018 && sd < 0.05)
                        {
                            string werhugol = "45 degrees";
                            //Console.WriteLine("Recommended angle for upper cone = " + werhugol);
                        }

                        double alfw = double.Parse(Console.ReadLine());// наверное угол конусности вверху 
                        double alfn = double.Parse(Console.ReadLine());// /-/ внизу

                        // Convert angles to radians
                        double alw = Pi * alfw / 180;
                        double alf = Pi * alfn / 180;

                        double dk = d1 * Math.Sqrt((1 / (md1 * md1) - 1 - 2.28 * rps / d1 + 0.07 * alfn * rps / d1 + 0.56 * (rps / d1) * (rps / d1)) * Math.Sin(alf) + 1);
                        dk = Math.Round(dk, 1);
                        double hk = (dk - dm1) / 2 / Math.Tan(alf);
                        hk = Math.Round(hk, 1);
                        double dw = 0.9 * d0;
                        dw = Math.Round(dw, 1);
                        double hw = (dw - dk) / (2 * Math.Tan(alw));
                        hw = Math.Round(hw, 1);
                        double rw = 0.05 * d0;
                        rw = Math.Round(rw, 1);
                        double rs = (d0 - dm1) / 3;
                        rs = Math.Round(rs, 1);
                        double r = (d0 - d1) / 2;
                        double md12 = d1 / dk;
                        double md11 = dk / d0;
                        double fik = (alw - alf) / 2;
                        double psi = 1 - dk / d0;
                        double psisr = 1 - Math.Sqrt(md1);

                        double sigms = sigmb * Math.Pow(psisr / psir, psir / (1 - psir)) / (1 - psir);
                        sigms = Math.Round(sigms, 1);
                        double sigmr = 1.1 * sigms * (((1 + mum / Math.Tan(alw)) * (Math.Log(1 / md11) - psi) + s / (2 * d1 * Math.Sqrt(md11))) * (1 + mum * fik) + Math.Log(1 / md12));
                        sigmr = Math.Round(sigmr, 1);

                        if (ms1 == 1)
                        {
                            // усилие при свертке без утонения
                            double pbu = Pi * d1 * s * sigmr;
                            pbu = Math.Round(pbu, 0);
                            Console.WriteLine("Усилие свертки = " + pbu + " kg"); // Только для консоли
                        }
                        else
                        {
                            double sigms2 = sigmas2(sigmb, md1, ms1, psir);
                            sigms2 = Math.Round(sigms2, 1);
                            double sigmz = sigmaz(mum, sigmr, sigms2, alf, ms1);
                            sigmz = Math.Round(sigmz, 1);
                            //усилие при свертке с утонением
                            double pu = Pi * d1 * s * sigmz;
                            pu = Math.Round(pu, 0);
                            // конечная стадия
                            double msk = ms1 * Math.Sqrt(md1);
                            double sigmrk = 0;
                            double sigms2k = sigmas2(sigmb, md1, msk, psir);
                            sigms2k = Math.Round(sigms2k, 1);
                            double sigmzk = sigmazk(mum, msk, alf, sigms2k);
                            sigmzk = Math.Round(sigmzk, 1);
                            double pk = Pi * d1 * s * sigmzk;
                            pk = Math.Round(pk, 0);
                            //Console.WriteLine("Final stage");
                            //Console.WriteLine("sigms = " + sigms2k + " kg / mm^2");
                            //Console.WriteLine("sigmz = " + sigmzk + " kg / mm^2");
                            //Console.WriteLine("Force of rolling = " + pk + " kg");
                        }
                        //Console.WriteLine("sigms = " + sigms + " kg / mm^2");
                        //Console.WriteLine("sigmr = " + sigmr + " kg / mm^2");

                        //Console.WriteLine("Двухконус. матрица");
                        //Console.WriteLine("Диам. верхнего конуса", dw);
                        //Console.WriteLine("Диаметр нижнего конуса", dk);
                        //Console.WriteLine("Высота верхнего конуса", hw);
                        //Console.WriteLine("Высота нижнего конуса", hk);
                        //Console.WriteLine("Угол верхнего конуса", alfw);
                        //Console.WriteLine("угол нижнего конуса", alfn);
                        //Console.WriteLine("Радиус входн.верх конуса", rw);
                        //Console.WriteLine("Радиус сопряж. конусов", rs);
                    }
                    else if (tipKonMatriz == "or")
                    {
                        //Console.WriteLine("одноконМатрРадиус");
                        // Define constants
                        double mum = 0.05;
                        double Pi = Math.PI;

                        // Get input values
                        //double d0 = double.Parse(Console.ReadLine());
                        //double d1 = double.Parse(Console.ReadLine());
                        double dm1 = double.Parse(Console.ReadLine());
                        // double rp = double.Parse(Console.ReadLine());
                        //double s = double.Parse(Console.ReadLine());
                        //double ms1 = double.Parse(Console.ReadLine());
                        //double s1 = double.Parse(Console.ReadLine());
                        double rps = rp + s / 2;
                        //double sigmb = double.Parse(Console.ReadLine());

                        //Введи угол  конуса
                        double alfn = double.Parse(Console.ReadLine());
                        double alf = Pi * alfn / 180;

                        // Calculate values
                        double dk = d1 * Math.Sqrt((1 / md1 * md1 - 1 - 2.28 * rps / d1 + 0.07 * alfn * rps / d1 + 0.56 * (rps / d1) * (rps / d1)) * Math.Sin(alf) + 1);
                        dk = Math.Round(dk, 1);
                        double hk = (dk - dm1) / 2 / Math.Tan(alf);
                        hk = Math.Round(hk, 1);
                        double md11 = dk / d0;
                        double a = md11 + sd;
                        double b = (1 - Math.Sin(alf)) * Math.Tan(alf);
                        double hm = (a * (1 - b) + b - sd * ms1 - md1) * d0 / (2 * Math.Tan(alf));
                        hm = Math.Round(hm, 1);
                        double c = hm - hk;
                        if (c < 0)
                        {
                            //Console.WriteLine("высота матр. меньше высоты конуса");
                            return;
                        }
                        double dkm = dm1 + 2 * hm * Math.Tan(alf);
                        dkm = Math.Round(dkm, 1);
                        double md12 = d1 / dk;
                        double rw = (d0 - dk - s) / 2;
                        rw = Math.Round(rw, 1);
                        double psi = 1 - dk / d0;
                        double psisr = 1 - Math.Sqrt(md1);
                        double sigms = sigmb * Math.Pow((psisr / psir), (psir / (1 - psir))) / (1 - psir);
                        sigms = Math.Round(sigms, 1);
                        double sigmr = 1.1 * sigms * (1 + mum / Math.Tan(alf)) * Math.Log(1 / md12);
                        sigmr = Math.Round(sigmr, 1);
                        if (ms1 == 1)
                        {
                            // усилие при свертке без утонения
                            double pbu = Math.PI * d1 * s * sigmr;
                            pbu = Math.Round(pbu, 0);
                            //Console.WriteLine("sigms = " + sigms + " kg / mm^2");
                            //Console.WriteLine("sigmr = " + sigmr + " kg / mm^2");
                            //Console.WriteLine("усилие свертки = " + pbu + " kg");
                        }
                        else
                        {
                            double sigms2 = sigmas2(sigmb, md1, ms1, psir);
                            sigms2 = Math.Round(sigms2, 1);
                            double sigmz = sigmaz(mum, sigmr, sigms2, alf, ms1);
                            sigmz = Math.Round(sigmz, 1);
                            //усилие при свертке с утонением
                            double pu = Pi * d1 * s1 * sigmz;
                            pu = Math.Round(pu, 0);
                            // конечная стадия
                            double msk = ms1 * Math.Sqrt(md1);
                            double sigmrk = 0;
                            double sigms2k = sigmas2(sigmb, md1, msk, psir);
                            sigms2k = Math.Round(sigms2k, 1);
                            double sigmzk = sigmazk(mum, msk, alf, sigms2k);
                            sigmzk = Math.Round(sigmzk, 1);
                            //усилие при свертке с утонением в конечн. стадии
                            double pk = Pi * d1 * s1 * sigmzk;
                            pk = Math.Round(pk, 0);

                            //Console.WriteLine("sigms = " + sigms2 + " kg / mm^2");
                            //Console.WriteLine("sigmz = " + sigmz + " kg / mm^2");
                            //Console.WriteLine("усилие свертки = " + pu + " kg");
                            //Console.WriteLine("конечная стадия");
                            //Console.WriteLine("sigms = " + sigms2k + " kg / mm^2");
                            //Console.WriteLine("sigmz = " + sigmzk + " kg / mm^2");
                            //Console.WriteLine("усилие свертки = " + pk + " kg");
                        }
                        //Console.WriteLine("Диам.  конуса=" + dkm);
                        //Console.WriteLine("Высота конуса=" + hm);
                        //Console.WriteLine("угол  конуса=" + alfn);
                        //Console.WriteLine("радиус конуса=" + rw);
                    }
                }
            }
            // Разветвление радиальной
            else if (tipMatriz == "r")
            {
                if (sd > (1 - md1) / 18)
                {
                    //Console.WriteLine("радМатрБезПриж");
                    // Опять же ввод
                    double mum = 0.05;
                    double Pi = Math.PI;
                    // double d0 = double.Parse(Console.ReadLine());
                    // double d1 = double.Parse(Console.ReadLine());
                    double dm1 = double.Parse(Console.ReadLine());
                    // double rp = double.Parse(Console.ReadLine());
                    //double s = double.Parse(Console.ReadLine());
                    //double ms1 = double.Parse(Console.ReadLine());
                    // s1 = double.Parse(Console.ReadLine());
                    double rps = rp + s / 2;
                    //double sigmb = double.Parse(Console.ReadLine());


                    // Calculate the radius of the matrix.
                    double rms = d1 * (1 - md1) / (2 * md1);
                    double rm = rms - s1 / 2;
                    rm = Math.Round(rm, 1);

                    // Calculate the other parameters.
                    double fig = 200 * (1 - md1 * (1 + 2.28 * rps / d1)) / (1 + md1 * (5 - 6 * md1));
                    double fi = Pi * fig / 180;
                    double md11 = md1 * (1 + 2 * rms * (1 - Math.Cos(fi)) / d1);
                    double md12 = md1 / md11;
                    double alr = Math.Atan(Math.Sqrt((rm + s) * (rm + s) - (rm + s1) * (rm + s1)) / (rm + s1));
                    double al = alr / 2;
                    double psisr = 1 - Math.Sqrt(md1);
                    double sigms = sigmb * (psisr / psir) * Math.Pow(psir / (1 - psir), 1 / (1 - psir)) / (1 - psir);
                    sigms = Math.Round(sigms, 1);
                    double sigmr = 1.1 * sigms * (1 + mum * fi) * Math.Log(1 / md12);
                    sigmr = Math.Round(sigmr, 1);

                    // Calculate the required forces.
                    double pbu, pu;
                    if (ms1 == 1)
                    {
                        //усилие при свертке без утонения
                        pbu = Pi * d1 * s * sigmr;
                        pbu = Math.Round(pbu, 0);
                        //Console.WriteLine("sigms = " + sigms + " kg / mm^2");
                        //Console.WriteLine("sigmr = " + sigmr + " kg / mm^2");
                        //Console.WriteLine("усилие свертки = " + pbu + " kg");
                    }
                    else
                    {
                        double sigms2 = sigmas2(sigmb, md1, ms1, psir);
                        sigms2 = Math.Round(sigms2, 1);
                        double sigmz = sigmaz(mum, sigmr, sigms2, al, ms1);
                        sigmz = Math.Round(sigmz, 1);
                        //усилие при свертке с утонением
                        pu = Pi * d1 * s1 * sigmz;
                        pu = Math.Round(pu, 0);
                        // конечная стадия
                        double msk = ms1 * Math.Sqrt(md1);
                        double sigmrk = 0;
                        double sigms2k = sigmas2(sigmb, md1, msk, psir);
                        sigms2k = Math.Round(sigms2k, 1);
                        double sigmzk = sigmazk(mum, msk, al, sigms2k);
                        sigmzk = Math.Round(sigmzk, 1);
                        //усилие при свертке с утонением в конечн. стадии
                        double pk = Pi * d1 * s1 * sigmzk;
                        pk = Math.Round(pk, 0);

                        //Console.WriteLine("sigms = " + sigms2 + " kg / mm^2");
                        //Console.WriteLine("sigmz = " + sigmz + " kg / mm^2");
                        //Console.WriteLine("усилие свертки = " + pu + " kg");
                        //Console.WriteLine("конечная стадия");
                        //Console.WriteLine("sigms = " + sigms2k + " kg / mm^2");
                        //Console.WriteLine("sigmz = " + sigmzk + " kg / mm^2");
                        //Console.WriteLine("усилие свертки = " + pk + " kg");
                    }

                    // Print the results.
                    Console.WriteLine("Радиальная матрица с прижимом");
                    if (sd < (1 - psir * Math.Log(1 / md1) - md1) / 20)
                    {
                        //Console.WriteLine("Требуется плоский и торцевой прижим");
                    }
                    else
                    {
                        //Console.WriteLine("Плоский прижим");
                    }
                    //Console.WriteLine("Радиус матрицы = " + rm);
                }
                else
                {
                    //Console.WriteLine("радМатрПрижим");
                    // Define the constants.
                    const double mum = 0.05;
                    const double Pi = Math.PI;

                    // Опять же ввод
                    //double d0 = double.Parse(Console.ReadLine());
                    //double d1 = double.Parse(Console.ReadLine());
                    double dm1 = double.Parse(Console.ReadLine());
                    //double rp = double.Parse(Console.ReadLine());
                    //double s = double.Parse(Console.ReadLine());
                    //double ms1 = double.Parse(Console.ReadLine());
                    //double s1 = double.Parse(Console.ReadLine());
                    double rps = rp + s / 2;
                    //double sigmb = double.Parse(Console.ReadLine());

                    // Calculate the radius of the matrix.
                    double rms = 0.69 * d1 * (0.16 * Math.Sqrt(18.3 / md1 * md1 + 21.2 + 10.2 * (rps / d1) * (rps / d1) - 41.3 * rps / d1) - 1);
                    double rm = rms - s1 / 2;
                    rm = Math.Round(rm, 1);

                    // Calculate the reduced modulus of deformation.
                    double md11 = (d1 + 2 * rms) / d0;
                    double md12 = md1 / md11;
                    double alr = Math.Atan2(Math.Sqrt((rm + s) * (rm + s) - (rm + s1) * (rm + s1)), rm + s1);
                    double fi = Pi / 2 - alr;
                    double al = alr / 2;
                    double psisr = 1 - Math.Sqrt(md1);
                    double sigms = sigmb * (psisr / psir) * Math.Pow(psir / (1 - psir), 1 / (1 - psir)) / (1 - psir);
                    sigms = Math.Round(sigms, 1);

                    // Calculate the shear modulus.
                    double a = Math.Log(1 / md1) + s / (4 * rm);
                    double b = 1 + 1.5 * mum;
                    double c = 0.2 * mum * b / md1;
                    double d = 1 - 18 * sd / (1 - md1);
                    double sigmr = 1.1 * sigms * a * b / (1 - c * d);
                    sigmr = Math.Round(sigmr, 1);

                    // Calculate the rolling force without thinning.
                    double pbu;

                    // Calculate the rolling force with thinning.
                    if (ms1 == 1)
                    {
                        //усилие при свертке без утонения.
                        pbu = Pi * d1 * s * sigmr;
                        pbu = Math.Round(pbu, 0);
                        //Console.WriteLine("sigms = " + sigms + " kg / mm^2");
                        //Console.WriteLine("sigmr = " + sigmr + " kg / mm^2");
                        //Console.WriteLine("усилие свертки = " + pbu + " kg");
                    }
                    else
                    {
                        double sigms2 = sigmas2(sigmb, md1, ms1, psir);
                        sigms2 = Math.Round(sigms2, 1);
                        double sigmz = sigmaz(mum, sigmr, sigms2, al, ms1);
                        sigmz = Math.Round(sigmz, 1);
                        //усилие при свертке с утонением
                        double pu = Pi * d1 * s1 * sigmz;
                        pu = Math.Round(pu, 0);
                        //конечная стадия
                        // Calculate the rolling force with thinning in the final stage.
                        double msk = ms1 * Math.Sqrt(md1);
                        double sigmrk = 0;
                        double sigms2k = sigmas2(sigmb, md1, msk, psir);
                        sigms2k = Math.Round(sigms2k, 1);
                        double sigmzk = sigmazk(mum, msk, al, sigms2k);
                        sigmzk = Math.Round(sigmzk, 1);
                        //усилие при свертке с утонением в конечн. стадии
                        double pk = Pi * d1 * s1 * sigmzk;
                        pk = Math.Round(pk, 0);

                        //Console.WriteLine("sigms = " + sigms2 + " kg / mm^2");
                        //Console.WriteLine("sigmz = " + sigmz + " kg / mm^2");
                        //Console.WriteLine("усилие свертки = " + pu + " kg");
                        //Console.WriteLine("конечная стадия");
                        //Console.WriteLine("sigms = " + sigms2k + " kg / mm^2");
                        //Console.WriteLine("sigmz = " + sigmzk + " kg / mm^2");
                        //Console.WriteLine("усилие свертки = " + pk + " kg");
                    }

                    //Console.WriteLine("Радиальная матрица с прижимом");
                    if (sd < (1 - psir * Math.Log(1 / md1) - md1) / 20)
                    {
                        //Console.WriteLine("Требуется плоский и торцевой прижим");
                    }
                    else
                    {
                        //Console.WriteLine("Плоский прижим");
                    }
                    //Console.WriteLine("Радиус матрицы = " + rm);
                }
            }
        }
        public static double sigmaz(double mum, double sigmr, double sigms2, double al, double ms1)
        {
            return ((1 + mum * (1 - sigmr / (1.15 * sigms2)) / Math.Sin(al) - mum * Math.Log(1 / ms1) / Math.Sin(al)) * Math.Log(1 / ms1) + sigmr / (1.15 * sigms2) + Math.Sin(al) / 2) * 1.15 * sigms2;
        }

        public static double sigmas2(double sigmb, double md1, double ms1, double psir)
        {
            return sigmb * Math.Pow((1 - md1 * ms1) / psir, psir / (1 - psir));
        }

        public static double sigmazk(double mum, double msk, double al, double sigms2k)
        {
            return 1.15 * sigms2k * ((1 + mum * (1 - Math.Log(1 / msk)) / Math.Sin(al)) * Math.Log(1 / msk) + Math.Sin(al) / 2);
        }
    }
}



/* HA ||0T0M


//Расчет технологических усилий при вытяжке

//Расчет удельного усилия
public static List<double> SpecificForce(double AngleAlpha, List<double> DegreeDeformationLS, int AmountPulling, double FrictionCoefficient,
                                         List<double> IntensityOfTension_US, double IntensityOfTension_LS)
{
    int c; double SpecificForce = 1; double ThicknessInitial0 = ThicknessInitial;
    List<double> SpecForce = new List<double>();

    for (c = 0; AmountPulling > 0; c++)
    {


        SpecificForce = ((Math.Pow(3, 0.5)) / 2) * IntensityOfTension_LS[c] * DegreeDeformationLS[c] * (2.2 * Math.Pow((Math.Cos(AngleAlpha) - 1), 2)) + (FrictionCoefficient / Math.Sin(AngleAlpha));
        SpecificForce = Math.Round(SpecificForce, 2);
        SpecForce.Add(SpecificForce);
        AmountPulling -= 1;

    }
    return SpecForce;
}
//Площадь нижнего расчетного сечения пуансона
public static List<double> AreaLowerDesignSection(List<double> OutsideDiameter, List<double> InsideDiameterLS, int AmountPulling,)

{
    int c; double AreaLowerDesignSection = 1;
    List<double> AreaLowDesignSection = new List<double>();

    for (c = 0; AmountPulling > 0; c++)
    {
        AreaLowerDesignSection = (Math.PI * (Math.Pow((OutsideDiameter[c] - InsideDiameterLS[c]), 2)) / 4;
        AreaLowerDesignSection = Math.Round(AreaLowerDesignSection, 2);
        AreaLowDesignSection.Add(AreaLowerDesignSection);
        AmountPulling -= 1;
    }
    return AreaLowDesignSection;
}
//Сила деформирования
public static List<double> DeformationForce(List<double> AreaLowDesignSection, List<double> SpecForce, int AmountPulling,)

{
    int c; double DeformationForce = 1;
    List<double> DeformForce = new List<double>();

    for (c = 0; AmountPulling > 0; c++)
    {
        DeformationForce = AreaLowDesignSection[c] * SpecForce;
        DeformationForce = Math.Round(DeformationForce, 2);
        DeformForce.Add(DeformationForce);
        AmountPulling -= 1;
    }
    return DeformForce;
}
}
*/