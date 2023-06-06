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
    public class UnitTestImpWebScrYear : UnitTestIImpWebScr
    {
        [TestInitialize]
        public override void Init()
        {
            // se initializeaza cu url -ul pentu anul dorit (1994)
            _url = "https://www.imdb.com/search/title/?title_type=feature&year=1994-01-01,1994-12-31";
            _implementationWebScrapler = new ImplementationWebScraperYears();
        }
    }
}
