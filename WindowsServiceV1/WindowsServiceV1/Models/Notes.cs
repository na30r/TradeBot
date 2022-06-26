using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeBot.Domain.Models.Enum;

namespace WindowsServiceV1.Models
{
    public class Notes
    {
        public readonly List<Note> MyNotes = new List<Note>();
        public Notes()
        {
            MyNotes.Add(new Note(CryptoType.bnb,ResolutionType._1h, "(stock) bnb 1H vaqan ghabele etemad ast."));
            MyNotes.Add(new Note(CryptoType.bnb,ResolutionType._30min, "(stock) nazdik be khate sefid va ghermez moamele nakon . hadeaghal 1 darsad fasele "));
            MyNotes.Add(new Note(null,ResolutionType._30min, "(stock) dar ravande noozooli sangin ba 30 min moamele nakon "));
            MyNotes.Add(new Note(null,ResolutionType._1h, "(stock) ghabl az position bastan moghavemate 30 min ro barresi kon - moghavemate kolli ro bedoon "));
        }
       
    }
    public class Note
    {
        public CryptoType? crypto { get; set; }
        public ResolutionType? TimeFrame { get; set; }
        public String MyNote { get; set; }

        public Note(CryptoType? cryptoType , ResolutionType? timeFrame , string myNote)
        {
            crypto = cryptoType;
            TimeFrame = timeFrame;
            MyNote = myNote;
        }
    }
}
