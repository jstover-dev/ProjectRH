using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH.Firmware {
    public class FirmwareCursor {

        private int maxLength;

        public int Position { get; private set; }

        public FirmwareCursor(int maxLength) {
            this.maxLength = maxLength;
            this.Position = 0;
        }

        public FirmwareCursor Reset() {
            this.Position = 0;
            return this;
        }

        public FirmwareCursor Next() {
            Position++;
            CheckBounds();
            return this;
        }

        public FirmwareCursor Previous() {
            Position--;
            CheckBounds();
            return this;
        }

        public FirmwareCursor MoveTo(int pos) {
            this.Position = pos;
            CheckBounds();
            return this;
        }

        private void CheckBounds() {
            if (Position >= maxLength || Position < 0)
                throw new ArgumentException("MoveTo target position is out of range");
        }

    }
}
