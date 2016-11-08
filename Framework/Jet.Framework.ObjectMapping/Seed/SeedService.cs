using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Jet.Framework.ObjectMapping
{
    public class SeedService
    {
        public static readonly SeedService Instance = new SeedService();

        private SeedService()
        {
            
        }

        private SeedGeneratorBase m_Generator;
        public SeedGeneratorBase Generator
        {
            get
            {
                if (m_Generator == null)
                    m_Generator = new SimpleSeedGenerator();
                return m_Generator;
            }
            set
            {
                m_Generator = value;
            }
        }
    }
}
