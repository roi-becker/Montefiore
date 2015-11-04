using Windows.Devices.Gpio;

namespace MontefioreClient
{
    public class Board
    {
        # region Privates

        private int[] pinsIndex = { 5, 6, 13, 26, 18, 23, 24, 25 };
        private GpioPin[] pins;
        private GpioPinValue[] pinsValues;

        # endregion

        public bool TryOpenBoard()
        {
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                pins = null;
                return false;
            }

            pins = new GpioPin[pinsIndex.Length];
            pinsValues = new GpioPinValue[pinsIndex.Length];

            for (int i = 0; i < pinsIndex.Length; i++)
            {
                pins[i] = gpio.OpenPin(pinsIndex[i]);
                pinsValues[i] = GpioPinValue.High;
                pins[i].Write(pinsValues[i]);
                pins[i].SetDriveMode(GpioPinDriveMode.Output);
            }

            return true;
        }

        public void TryOpen(int type, int floor)
        {
            int i = ((floor - 1) * 2) + type - 1;

            if (pinsValues[i] == GpioPinValue.High)
            {
                pinsValues[i] = GpioPinValue.Low;
                pins[i].Write(pinsValues[i]);
            }
        }

        public void TryClose(int type, int floor)
        {
            int i = ((floor - 1) * 2) + type - 1;

            if (pinsValues[i] != GpioPinValue.High)
            {
                pinsValues[i] = GpioPinValue.High;
                pins[i].Write(pinsValues[i]);
            }
        }

        public void Dispose()
        {
            // Close pins? make IDisposable?
        }
    }
}
