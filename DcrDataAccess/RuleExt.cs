/** 
 * This file is part of the DcrDataAccess project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
**/

using Activant.P21.Extensions.BusinessRule;
using DcrDataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DcrDataAccess
{
    public class RuleExt : Rule
    {                
        private List<string> _errors = new List<string>();

        public override RuleResult Execute()
        {
            return new RuleResult { Success = true };
        }

        public SessionInfo GetSessionInfo()
        {
            return new SessionInfo(Data.Fields["global_server"].FieldValue,
                Data.Fields["global_database"].FieldValue,
                Data.Fields["global_user_id"].FieldValue);
        }

        public string GetDataFieldValue(string name)
        {
            try
            {
                return Data.Fields[name].FieldValue;
            }
            catch
            {
                _errors.Add(string.Format("{0} - not found", name));
            }
            return "";
        }

        public bool SetDataFieldValue(string name, string value)
        {
            try
            {
                Data.Fields[name].FieldValue = value;
                return true;
            }
            catch
            {
                _errors.Add(string.Format("{0} - not found", name));
            }
            return false;
        }

        public DataField GetDataField(string name)
        {
            try
            {
                return Data.Fields[name];
            }
            catch
            {
                _errors.Add(string.Format("{0} - not found", name));
            }
            return null;
        }

        public int GetErrorsCount()
        {
            return _errors.Count;
        }

        public string GetErrors()
        {
            StringBuilder builder = new StringBuilder();

            foreach (string error in _errors)
            {
                builder.AppendLine(error);
            }

            return builder.ToString();
        }

        public override string GetDescription()
        {
            return this.GetType().Name;
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }
    }
}
