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
         * <p>Encapsulates a set of error-correction blocks in one symbol version. Most versions will
         * use blocks of differing sizes within one version, so, this encapsulates the parameters for
         * each set of blocks. It also holds the number of error-correction codewords per block since it
         * will be the same across all blocks within one version.</p>
         */
        public sealed class ECBlocks
        {
            private readonly int ecCodewordsPerBlock;
            private readonly ECB[] ecBlocks;

            public ECBlocks(int ecCodewordsPerBlock, ECB ecBlocks)
            {
                this.ecCodewordsPerBlock = ecCodewordsPerBlock;
                this.ecBlocks = new ECB[] { ecBlocks };
            }

            public ECBlocks(int ecCodewordsPerBlock, ECB ecBlocks1, ECB ecBlocks2)
            {
                this.ecCodewordsPerBlock = ecCodewordsPerBlock;
                ecBlocks = new ECB[] { ecBlocks1, ecBlocks2 };
            }

            public int GetECCodewordsPerBlock()
            {
                return ecCodewordsPerBlock;
            }

            public int GetNumBlocks()
            {
                var total = 0;
                for (var i = 0; i < ecBlocks.Length; i++)
                {
                    total += ecBlocks[i].GetCount();
                }
                return total;
            }

            public int GetTotalECCodewords()
            {
                return ecCodewordsPerBlock * GetNumBlocks();
            }

            public ECB[] GetECBlocks()
            {
                return ecBlocks;
            }
        }
    }
}
