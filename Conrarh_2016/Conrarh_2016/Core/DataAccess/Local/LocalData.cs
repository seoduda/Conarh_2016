using Conarh_2016.Application.Domain;
using System;
using System.Collections.Generic;

namespace Conrarh_2016.Core.DataAccess.Local
{
    public  class LocalData
    {
        public static List<Exhibitor> getLocalExhibitorList()
        {
            List<Exhibitor> exhList = new List<Exhibitor>();
            List<string> strList = LocalLists.getExhibitorList();
            Dictionary<string, SponsorType> dict = getLocalSponsorDictionary();
            foreach (string s in strList)
            {
                string[] items = s.Split(';');
                Exhibitor exh = new Exhibitor();
                exh.Title = items[0].Trim();
                exh.SponsorTypeId = items[1].Trim();
                exh.Icon = items[2].Trim();
                exh.Id = Guid.NewGuid().ToString();
                exh.CreatedAtTime = DateTime.Now;
                exh.UpdatedAtTime = DateTime.Now;
                exh.SponsorType = dict[exh.SponsorTypeId];
                exhList.Add(exh);
            }
            return exhList;
        }

        public static List<SponsorType> getLocalSponsorList()
        {
            List<SponsorType> dataList = new List<SponsorType>();
            List<string> strList = LocalLists.getSponsorList();

            foreach (string s in strList)
            {
                string[] items = s.Split(';');
                SponsorType st = new SponsorType();
                st.Title = items[0].Trim();
                st.Color = items[1].Trim();
                st.Type = Int32.Parse(items[2].Trim());
                st.Id = st.Title.GetHashCode().ToString() + st.Type.GetHashCode().ToString();
                st.CreatedAtTime = DateTime.Now;
                st.UpdatedAtTime = DateTime.Now;
                dataList.Add(st);
            }

            return dataList;
        }

        public static Dictionary<string, SponsorType> getLocalSponsorDictionary()
        {
            Dictionary<string, SponsorType> dict = new Dictionary<string, SponsorType>();
            List<SponsorType> stList = getLocalSponsorList();
            foreach (SponsorType st in stList)
            {
                dict.Add(st.Type.ToString(), st);
            }

            return dict;
        }
        

    }
}