/**************************************************************************
 *                                                                        *
 *  File:        CommedyCommand.cs                                        *
 *  Copyright:   (c) 2023, Fodor Maria-Gabriela                           *
 *  E-mail:      maria-gabriela.fodor@student.tuiasi.ro                   *
 *  Description: Test Class containing the TestInitialize method Init     *
 *  inheriting the abstract class UnitTestIImpWebScr in wich more tests   *
 *  are implemented                                                       *                       
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawl;

namespace UnitTestWebCrawler
{
    [TestClass]
    public class UnitTestImpWebScrAll : UnitTestIImpWebScr
    {
        [TestInitialize]
        public override void Init()
        {
            // se initializeaza cu url -ul pentru top 250 filme

            _url = "https://www.imdb.com/chart/top?ref_=tt_awd";
            _implementationWebScrapler = new ImplementationWebScraperAll();
        }
    }
}
