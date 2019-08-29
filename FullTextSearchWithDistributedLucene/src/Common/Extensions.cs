// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using Lucene.Net.Documents;
using NCommerce.Common.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NCommerce.Common
{
    public static class Extensions
    {
        const int EXP_READER_FIELDS = 12;
        const int EXP_DOC_FIELDS = 7;

        public static Product GetProduct(this SqlDataReader dataReader)
        {
            var objs = new object[dataReader.FieldCount];
            var result = dataReader.GetValues(objs);
            try
            {
                if (dataReader.FieldCount != EXP_READER_FIELDS)
                    throw new Exception(dataReader.FieldCount + " fields exists in DataReader instead of " + EXP_READER_FIELDS);

                var product = new Product
                {
                    Name = dataReader.GetString(dataReader.GetOrdinal("name")),
                    Category = dataReader.GetString(dataReader.GetOrdinal("category")),
                    CategroryTree = dataReader.GetString(dataReader.GetOrdinal("category_tree")),
                    ID = dataReader.GetString(dataReader.GetOrdinal("id")),
                    RetailPrice = dataReader.GetInt32(dataReader.GetOrdinal("retail_price")),
                    DiscountedPrice = dataReader.GetInt32(dataReader.GetOrdinal("discounted_price")),
                    ImageUrls = dataReader["images"].GetImageUrls(),
                    Description = dataReader.GetString(dataReader.GetOrdinal("description")),
                    ProductRating = dataReader.GetString(dataReader.GetOrdinal("rating")),
                    OverallRating = dataReader.GetString(dataReader.GetOrdinal("overall_rating")),
                    Brand = dataReader.GetString(dataReader.GetOrdinal("brand")),
                    Sepecification = dataReader.GetString(dataReader.GetOrdinal("specifications"))
                };

                return product;
            }
            catch (Exception exp)
            {
                Console.WriteLine(objs[4]);
                throw exp;
            }
        }

        public static ProductFTS GetProductFTS(this SqlDataReader dataReader)
        {
            var objs = new object[dataReader.FieldCount];
            var result = dataReader.GetValues(objs);
            try
            {
                if (dataReader.FieldCount != EXP_READER_FIELDS)
                    throw new Exception(dataReader.FieldCount + " fields exists in DataReader instead of " + EXP_READER_FIELDS);

                var product = new ProductFTS
                {
                    Name = dataReader.GetString(dataReader.GetOrdinal("name")),
                    Category = dataReader.GetString(dataReader.GetOrdinal("category")),
                    ID = dataReader.GetString(dataReader.GetOrdinal("id")),
                    RetailPrice = dataReader.GetInt32(dataReader.GetOrdinal("retail_price")),
                    DiscountedPrice = dataReader.GetInt32(dataReader.GetOrdinal("discounted_price")),
                    Description = dataReader.GetString(dataReader.GetOrdinal("description")),
                    ImageUrl = dataReader["images"].GetImageUrl(),
                };

                return product;
            }
            catch (Exception exp)
            {
                Console.WriteLine(objs[4]);
                throw exp;
            }
        }

        public static ProductFTS GetProductFTS(this Document document)
        {
            try
            {
                if (document.Fields.Count < EXP_DOC_FIELDS)
                    throw new Exception(document.Fields.Count + " fields exists in DataReader instead of " + EXP_DOC_FIELDS);

                var product = new ProductFTS
                { 
                    Description = document.Get("description"),
                    Category = document.Get("category"),
                    ID = document.Get("id"),
                    Name = document.Get("name"),
                    RetailPrice = int.Parse(document.Get("retail_price")),
                    DiscountedPrice = int.Parse(document.Get("discounted_price")),
                    ImageUrl = document.Get("image"),
                };

                return product;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static Document GetLuceneDocument(this Product product)
        {
            try
            {
                Document doc = null;
                if (product != null)
                {
                    doc = new Document
                    {
                        new TextField("name", product.Name, Field.Store.YES),
                        new TextField("description", product.Description, Field.Store.YES),
                        //StringField store as is without perfroming the tokanization on this value
                        new StringField("category", product.Category, Field.Store.YES),
                        //StoreField stored but not anaylized & indexed
                        new StoredField("id", product.ID),
                        new StoredField("retail_price", product.RetailPrice),
                        new StoredField("discounted_price", product.DiscountedPrice),
                    };
                    if (product.ImageUrls != null && product.ImageUrls.Count > 0)
                        doc.Add(new StoredField("image", product.ImageUrls.GetImageUrl()));
                }

                return doc;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static Document GetLuceneDocument(this ProductFTS product)
        {
            try
            {
                Document doc = null;
                if (product != null)
                {
                    doc = new Document
                    {
                        new TextField("name", product.Name, Field.Store.YES),
                        new TextField("description", product.Description, Field.Store.YES),
                        //StringField store as is without perfroming the tokanization on this value
                        new StringField("category", product.Category, Field.Store.YES),
                        //StoreField stored but not anaylized & indexed
                        new StoredField("id", product.ID),
                        new StoredField("retail_price", product.RetailPrice),
                        new StoredField("discounted_price", product.DiscountedPrice),
                    };
                    if(product.ImageUrl != null)
                        doc.Add(new StoredField("image", product.ImageUrl));
                }

                return doc;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static string GetImageUrl(this object images)
        {
            if (images != null)
            {
                var imgsString = images.ToString();//it was stored as JSON Array
                if (string.IsNullOrEmpty(imgsString) == false)
                {
                    var jarray = JArray.Parse(imgsString);
                    foreach (var item in jarray)//individual image url
                    {
                        return item.ToString();
                    }
                }
            }

            return null;
        }

        public static IList<string> GetImageUrls(this object images)
        {
            var list = new List<string>();

            if (images != null)
            {
                var imgsString = images.ToString();//it was/should stored as JSON Array
                if (string.IsNullOrEmpty(imgsString) == false)
                {
                    var jarray = JArray.Parse(imgsString);

                    foreach (var item in jarray)//individual image url
                    {
                        list.Add(item.ToString());
                    }
                }
            }

            return list;
        }
    }
}
