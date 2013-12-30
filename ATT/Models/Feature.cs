﻿#region copyright
// Copyright 2013 Matthew S. Gerber (gerber.matthew@gmail.com)
// 
// This file is part of the Asymmetric Threat Tracker (ATT).
// 
// The ATT is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// The ATT is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with the ATT.  If not, see <http://www.gnu.org/licenses/>.
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using LAIR.ResourceAPIs.PostgreSQL;
using PTL.ATT.Models;
using LAIR.Collections.Generic;

namespace PTL.ATT.Models
{
    public class Feature : IComparable<Feature>
    {
        public const string Table = "feature";

        public class Columns
        {
            [Reflector.Insert, Reflector.Select(true)]
            public const string Description = "description";
            [Reflector.Insert, Reflector.Select(true)]
            public const string EnumType = "enum_type";
            [Reflector.Insert, Reflector.Select(true)]
            public const string EnumValue = "enum_value";
            [Reflector.Select(true)]
            public const string Id = "id";
            [Reflector.Insert, Reflector.Select(true)]
            public const string ModelId = "model_id";
            [Reflector.Insert, Reflector.Select(true)]
            public const string PredictionResourceId = "prediction_resource_id";
            [Reflector.Insert, Reflector.Select(true)]
            public const string TrainingResourceId = "training_resource_id";

            public static string Insert { get { return Reflector.GetInsertColumns(typeof(Columns)); } }
            public static string Select { get { return Reflector.GetSelectColumns(Table, typeof(Columns)); } }
        }

        [ConnectionPool.CreateTable(typeof(Prediction))]
        private static string CreateTable(ConnectionPool connection)
        {
            return "CREATE TABLE IF NOT EXISTS " + Table + " (" +
                    Columns.Description + " VARCHAR," +
                    Columns.EnumType + " VARCHAR," +
                    Columns.EnumValue + " VARCHAR," +
                    Columns.Id + " SERIAL PRIMARY KEY," +
                    Columns.ModelId + " INT REFERENCES " + DiscreteChoiceModel.Table + " ON DELETE CASCADE," +
                    Columns.PredictionResourceId + " VARCHAR," + 
                    Columns.TrainingResourceId + " VARCHAR);" + 
                    (connection.TableExists(Table) ? "" :
                    "CREATE INDEX ON " + Table + " (" + Columns.EnumType + ");" +
                    "CREATE INDEX ON " + Table + " (" + Columns.EnumValue + ");" +
                    "CREATE INDEX ON " + Table + " (" + Columns.ModelId + ");" +
                    "CREATE INDEX ON " + Table + " (" + Columns.PredictionResourceId + ");" +
                    "CREATE INDEX ON " + Table + " (" + Columns.TrainingResourceId + ");");
        }

        public static int Create(NpgsqlConnection connection, string description, Type enumType, Enum enumValue, DiscreteChoiceModel model, string trainingResourceId, string predictionResourceId, bool vacuum)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO " + Table + " (" + Columns.Insert + ") VALUES ('" + description + "','" + enumType + "','" + enumValue + "'," + model.Id + "," + (predictionResourceId == null ? "NULL" : "'" + predictionResourceId + "'") + "," + (trainingResourceId == null ? "NULL" : "'" + trainingResourceId + "'") + ") RETURNING " + Columns.Id, connection);
            int id = Convert.ToInt32(cmd.ExecuteScalar());

            if (vacuum)
                VacuumTable();

            return id;
        }

        internal static void VacuumTable()
        {
            DB.Connection.ExecuteNonQuery("VACUUM ANALYZE " + Table);
        }

        private int _id;
        private Type _enumType;
        private Enum _enumValue;
        private string _description;
        private int _modelId;
        private string _trainingResourceId;
        private string _predictionResourceId;

        public int Id
        {
            get {  return _id; }
        }

        public Type EnumType
        {
            get { return _enumType; }
        }

        public Enum EnumValue
        {
            get { return _enumValue; }
        }

        public string Description
        {
            get { return _description; }
        }

        public int ModelId
        {
            get { return _modelId; }
        }

        public string TrainingResourceId
        {
            get { return _trainingResourceId; }
        }

        public string PredictionResourceId
        {
            get { return _predictionResourceId; }
            set
            {
                _predictionResourceId = value;

                DB.Connection.ExecuteNonQuery("UPDATE " + Table + " SET " + Columns.PredictionResourceId + "='" + value + "' WHERE " + Columns.Id + "=" + _id);
            }
        }

        public string RemapKey
        {
            get { return _enumType + "-" + _enumValue + "-" + _trainingResourceId; }
        }

        internal Feature(NpgsqlDataReader reader)
        {
            Construct(reader);
        }

        public Feature(Type enumType, Enum enumValue, string trainingResourceId, string predictionResourceId, string description)
        {
            Construct(-1, -1, enumType, enumValue, trainingResourceId, predictionResourceId, description);
        }

        private void Construct(NpgsqlDataReader reader)
        {
            Type enumType = Reflection.GetType(Convert.ToString(reader[Table + "_" + Columns.EnumType]));

            Construct(Convert.ToInt32(reader[Table + "_" + Columns.Id]),
                      Convert.ToInt32(reader[Table + "_" + Columns.ModelId]),
                      enumType,
                      (Enum)Enum.Parse(enumType, Convert.ToString(reader[Table + "_" + Columns.EnumValue])),
                      Convert.ToString(reader[Table + "_" + Columns.TrainingResourceId]),
                      Convert.ToString(reader[Table + "_" + Columns.PredictionResourceId]),
                      Convert.ToString(reader[Table + "_" + Columns.Description]));
        }

        private void Construct(int id, int modelId, Type enumType, Enum enumValue, string trainingResourceId, string predictionResourceId, string description)
        {
            _id = id;
            _modelId = modelId;
            _enumType = enumType;
            _enumValue = enumValue;
            _description = description;
            _trainingResourceId = trainingResourceId == null ? "" : trainingResourceId;
            _predictionResourceId = predictionResourceId == null ? "" : predictionResourceId;
        }

        public override string ToString()
        {
            string enumStr = _enumType.ToString();
            enumStr = enumStr.Substring(enumStr.LastIndexOf("+") + 1);
            return _description + " (" + enumStr + ")" + (_predictionResourceId == _trainingResourceId ? "" : " --> " + _predictionResourceId);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Feature))
                return false;

            Feature f = obj as Feature;

            return _enumType == f.EnumType && _enumValue.ToString() == f.EnumValue.ToString() && _trainingResourceId == f.TrainingResourceId && _predictionResourceId == f.PredictionResourceId;
        }

        public override int GetHashCode()
        {
            return (_enumType + "-" + _enumValue + "-" + _trainingResourceId + "-" + _predictionResourceId).GetHashCode();
        }

        public int CompareTo(Feature other)
        {
            int cmp = _enumType.ToString().CompareTo(other.EnumType.ToString());

            if (cmp == 0)
                cmp = _enumValue.ToString().CompareTo(other.EnumValue.ToString());

            if (cmp == 0 && _trainingResourceId != null && other.TrainingResourceId != null)
            {
                int r1, r2;
                if (int.TryParse(_trainingResourceId, out r1) && int.TryParse(other.TrainingResourceId, out r2))
                    cmp = r1.CompareTo(r2);
                else
                    cmp = _trainingResourceId.CompareTo(other.TrainingResourceId);
            }

            if (cmp == 0 && _predictionResourceId != null && other.PredictionResourceId != null)
            {
                int r1, r2;
                if (int.TryParse(_predictionResourceId, out r1) && int.TryParse(other.PredictionResourceId, out r2))
                    cmp = r1.CompareTo(r2);
                else
                    cmp = _predictionResourceId.CompareTo(other.PredictionResourceId);
            }

            return cmp;
        }
    }
}