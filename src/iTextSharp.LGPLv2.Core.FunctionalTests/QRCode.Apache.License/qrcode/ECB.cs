/*
 * Copyright 2007 ZXing authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace iTextSharp.text.pdf.qrcode
{

    public sealed partial class Version
    {
        /**
         * <p>Encapsualtes the parameters for one error-correction block in one symbol version.
         * This includes the number of data codewords, and the number of times a block with these
         * parameters is used consecutively in the QR code version's format.</p>
         */

        public sealed class ECB
        {
            private readonly int count;
            private readonly int dataCodewords;

            public ECB(int count, int dataCodewords)
            {
                this.count = count;
                this.dataCodewords = dataCodewords;
            }

            public int GetCount()
            {
                return count;
            }

            public int GetDataCodewords()
            {
                return dataCodewords;
            }
        }
    }
}