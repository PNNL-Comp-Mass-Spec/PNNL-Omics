using System.Collections.Generic;

namespace PNNLOmics.Data.Constants.Libraries
{
    /// <summary>
    /// This is a Class designed to allow for users to add data not in any library
    /// </summary>
    public class UserUnitLibrary : MatterLibrary<UserUnit, UserUnitName>
    {
            
        /// <summary>
        /// Loads the information from the const section into a user specified library
        /// </summary>
        public override void LoadLibrary()
        {
            m_symbolToCompoundMap = new Dictionary<string, UserUnit>();
            m_enumToSymbolMap = new Dictionary<UserUnitName, string>();

            var user01 = new UserUnit();
            user01.Name = "User01";
            user01.MassMonoIsotopic = 0;
            user01.Symbol = "U01";
            user01.UserUnitType = UserUnitName.User01;

            var user02 = new UserUnit();
            user02.Name = "User02";
            user02.MassMonoIsotopic = 0;
            user02.Symbol = "U02";
            user02.UserUnitType = UserUnitName.User02;

            var user03 = new UserUnit();
            user03.Name = "User03";
            user03.MassMonoIsotopic = 0;
            user03.Symbol = "U03";
            user03.UserUnitType = UserUnitName.User03;

            var user04 = new UserUnit();
            user04.Name = "User04";
            user04.MassMonoIsotopic = 0;
            user04.Symbol = "U04";
            user04.UserUnitType = UserUnitName.User04;

            var user05 = new UserUnit();
            user05.Name = "User05";
            user05.MassMonoIsotopic = 0;
            user05.Symbol = "U05";
            user05.UserUnitType = UserUnitName.User05;

            var user06 = new UserUnit();
            user06.Name = "User06";
            user06.MassMonoIsotopic = 0;
            user06.Symbol = "U06";
            user06.UserUnitType = UserUnitName.User06;

            var user07 = new UserUnit();
            user07.Name = "User07";
            user07.MassMonoIsotopic = 0;
            user07.Symbol = "U07";
            user07.UserUnitType = UserUnitName.User07;

            var user08 = new UserUnit();
            user08.Name = "User08";
            user08.MassMonoIsotopic = 0;
            user08.Symbol = "U08";
            user08.UserUnitType = UserUnitName.User08;

            var user09 = new UserUnit();
            user09.Name = "User09";
            user09.MassMonoIsotopic = 0;
            user09.Symbol = "U09";
            user09.UserUnitType = UserUnitName.User09;

            var user10 = new UserUnit();
            user10.Name = "User10";
            user10.MassMonoIsotopic = 0;
            user10.Symbol = "U10";
            user10.UserUnitType = UserUnitName.User10;

            
            m_symbolToCompoundMap.Add(user01.Symbol, user01);
            m_symbolToCompoundMap.Add(user02.Symbol, user02);
            m_symbolToCompoundMap.Add(user03.Symbol, user03);
            m_symbolToCompoundMap.Add(user04.Symbol, user04);
            m_symbolToCompoundMap.Add(user05.Symbol, user05);
            m_symbolToCompoundMap.Add(user06.Symbol, user06);
            m_symbolToCompoundMap.Add(user07.Symbol, user07);
            m_symbolToCompoundMap.Add(user08.Symbol, user08);
            m_symbolToCompoundMap.Add(user09.Symbol, user09);
            m_symbolToCompoundMap.Add(user10.Symbol, user10);

            m_enumToSymbolMap.Add(UserUnitName.User01, user01.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User02, user02.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User03, user03.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User04, user04.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User05, user05.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User06, user06.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User07, user07.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User08, user08.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User09, user09.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User10, user10.Symbol);  
        }
        /// <summary>
        /// Creates a UserUnitLibrary from Usernits so that it can be stored as a singleton with Constants.SetUserUnitLibrary(myLibrary);
        /// UserUnit.Symbol must be defined and UserUnit.UserUnitType (which is the UserUnitName)
        /// </summary>
        /// <param name="user1"></param>
        /// <returns></returns>
        public void SetLibrary(UserUnit user1)
        {
            var library = new UserUnitLibrary();
            library.m_symbolToCompoundMap.Add(user1.Symbol, user1);     
    
            library.m_enumToSymbolMap.Add(user1.UserUnitType, user1.Symbol);

            m_enumToSymbolMap = library.m_enumToSymbolMap;
            m_symbolToCompoundMap = library.m_symbolToCompoundMap;
            Constants.SetUserUnitLibrary(library);
        }

        /// <summary>
        /// Creates a UserUnitLibrary from Usernits so that it can be stored as a singleton with Constants.SetUserUnitLibrary(myLibrary);
        /// UserUnit.Symbol must be defined and UserUnit.UserUnitType (which is the UserUnitName)
        /// </summary>
        /// <param name="user1"></param>
        /// <returns></returns>
        public void SetLibrary(UserUnit user1, UserUnit user2)
        {
            var library = new UserUnitLibrary();
            library.m_symbolToCompoundMap.Add(user1.Symbol, user1);
            library.m_symbolToCompoundMap.Add(user2.Symbol, user2);

            library.m_enumToSymbolMap.Add(user1.UserUnitType, user1.Symbol);
            library.m_enumToSymbolMap.Add(user2.UserUnitType, user2.Symbol);

            m_enumToSymbolMap = library.m_enumToSymbolMap;
            m_symbolToCompoundMap = library.m_symbolToCompoundMap;
            Constants.SetUserUnitLibrary(library);
        }

        /// <summary>
        /// Creates a UserUnitLibrary from Usernits so that it can be stored as a singleton with Constants.SetUserUnitLibrary(myLibrary);
        /// UserUnit.Symbol must be defined and UserUnit.UserUnitType (which is the UserUnitName)
        /// </summary>
        /// <param name="user1"></param>
        /// <returns></returns>
        public void SetLibrary(UserUnit user1, UserUnit user2, UserUnit user3)
        {
            var library = new UserUnitLibrary();

            library.m_symbolToCompoundMap = new Dictionary<string, UserUnit>();
            library.m_symbolToCompoundMap.Add(user1.Symbol, user1);
            library.m_symbolToCompoundMap.Add(user2.Symbol, user2);
            library.m_symbolToCompoundMap.Add(user3.Symbol, user3);

            library.m_enumToSymbolMap = new Dictionary<UserUnitName, string>();
            library.m_enumToSymbolMap.Add(user1.UserUnitType, user1.Symbol);
            library.m_enumToSymbolMap.Add(user2.UserUnitType, user2.Symbol);
            library.m_enumToSymbolMap.Add(user3.UserUnitType, user3.Symbol);

            m_enumToSymbolMap = library.m_enumToSymbolMap;
            m_symbolToCompoundMap = library.m_symbolToCompoundMap;
            Constants.SetUserUnitLibrary(library);
        }
    }
}
