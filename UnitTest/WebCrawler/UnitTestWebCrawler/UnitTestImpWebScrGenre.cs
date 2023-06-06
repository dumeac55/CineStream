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
    public class UnitTestImpWebScrGenre : UnitTestIImpWebScr
    {
        [TestInitialize]
        public override void Init()
        {
            // se initializeaza cu url -ul pentru top filem din genul drama

            _url = "https://www.imdb.com/search/title/?genres=Drama&sort=user_rating,desc&title_type=feature&num_votes=25000,&pf_rd_m=A2FGELUUNOQJNL&pf_rd_p=94365f40-17a1-4450-9ea8-01159990ef7f&pf_rd_r=MWWKWPHG5022Y3561KDP&pf_rd_s=right-6&pf_rd_t=15506&pf_rd_i=top&ref_=chttp_gnr_5";
            _implementationWebScrapler = new ImplementationWebScraperGenres();
        }
    }
}
