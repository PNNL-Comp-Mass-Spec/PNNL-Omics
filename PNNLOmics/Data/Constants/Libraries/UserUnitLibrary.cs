using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            UserUnit user1 = new UserUnit();
            user1.Name = "User01";
            user1.MassMonoIsotopic = 0;
            user1.Symbol = "U01";
            user1.UserUnitType = UserUnitName.User01;

            UserUnit user2 = new UserUnit();
            user2.Name = "User02";
            user2.MassMonoIsotopic = 0;
            user2.Symbol = "U02";
            user2.UserUnitType = UserUnitName.User02;

            UserUnit user3 = new UserUnit();
            user3.Name = "User03";
            user3.MassMonoIsotopic = 0;
            user3.Symbol = "U03";
            user3.UserUnitType = UserUnitName.User03;

            m_symbolToCompoundMap.Add(user1.Symbol, user1);
            m_symbolToCompoundMap.Add(user2.Symbol, user2);
            m_symbolToCompoundMap.Add(user3.Symbol, user3);

            m_enumToSymbolMap.Add(UserUnitName.User01, user1.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User02, user2.Symbol);
            m_enumToSymbolMap.Add(UserUnitName.User03, user3.Symbol);
                
        }
        /// <summary>
        /// Creates a UserUnitLibrary from Usernits so that it can be stored as a singleton with Constants.SetUserUnitLibrary(myLibrary);
        /// UserUnit.Symbol must be defined and UserUnit.UserUnitType (which is the UserUnitName)
        /// </summary>
        /// <param name="user1"></param>
        /// <returns></returns>
        public UserUnitLibrary SetLibrary(UserUnit user1)
        {
            UserUnitLibrary library = new UserUnitLibrary();
            library.m_symbolToCompoundMap.Add(user1.Symbol, user1);     
    
            library.m_enumToSymbolMap.Add(user1.UserUnitType, user1.Symbol);

            this.m_enumToSymbolMap = library.m_enumToSymbolMap;
            this.m_symbolToCompoundMap = library.m_symbolToCompoundMap;
            Constants.SetUserUnitLibrary(library);
            return library;
        }

        /// <summary>
        /// Creates a UserUnitLibrary from Usernits so that it can be stored as a singleton with Constants.SetUserUnitLibrary(myLibrary);
        /// UserUnit.Symbol must be defined and UserUnit.UserUnitType (which is the UserUnitName)
        /// </summary>
        /// <param name="user1"></param>
        /// <returns></returns>
        public UserUnitLibrary SetLibrary(UserUnit user1, UserUnit user2)
        {
            UserUnitLibrary library = new UserUnitLibrary();
            library.m_symbolToCompoundMap.Add(user1.Symbol, user1);
            library.m_symbolToCompoundMap.Add(user2.Symbol, user2);

            library.m_enumToSymbolMap.Add(user1.UserUnitType, user1.Symbol);
            library.m_enumToSymbolMap.Add(user2.UserUnitType, user2.Symbol);

            this.m_enumToSymbolMap = library.m_enumToSymbolMap;
            this.m_symbolToCompoundMap = library.m_symbolToCompoundMap;
            Constants.SetUserUnitLibrary(library);
            return library;
        }

        /// <summary>
        /// Creates a UserUnitLibrary from Usernits so that it can be stored as a singleton with Constants.SetUserUnitLibrary(myLibrary);
        /// UserUnit.Symbol must be defined and UserUnit.UserUnitType (which is the UserUnitName)
        /// </summary>
        /// <param name="user1"></param>
        /// <returns></returns>
        public UserUnitLibrary SetLibrary(UserUnit user1, UserUnit user2, UserUnit user3)
        {
            UserUnitLibrary library = new UserUnitLibrary();

            library.m_symbolToCompoundMap = new Dictionary<string, UserUnit>();
            library.m_symbolToCompoundMap.Add(user1.Symbol, user1);
            library.m_symbolToCompoundMap.Add(user2.Symbol, user2);
            library.m_symbolToCompoundMap.Add(user3.Symbol, user3);

            library.m_enumToSymbolMap = new Dictionary<UserUnitName, string>();
            library.m_enumToSymbolMap.Add(user1.UserUnitType, user1.Symbol);
            library.m_enumToSymbolMap.Add(user2.UserUnitType, user2.Symbol);
            library.m_enumToSymbolMap.Add(user3.UserUnitType, user3.Symbol);

            this.m_enumToSymbolMap = library.m_enumToSymbolMap;
            this.m_symbolToCompoundMap = library.m_symbolToCompoundMap;
            Constants.SetUserUnitLibrary(library);
            
            return library;
        }
    }
}
