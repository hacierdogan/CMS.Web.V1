using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class SocialMediaDAL : DataAccess
    {
        public static List<SocialMedia> GetSocialMedia()
        {
            List<SocialMedia> contentList = new List<SocialMedia>();
            DataTable dt = DAL.GetListSocialMedia();
            foreach (DataRow row in dt.Rows)
            {
                SocialMedia social = new SocialMedia();
                {
                    social.FacebookUrl = row.Field<string>("FacebookUrl");
                    social.IsFacebook = row.Field<bool>("IsFacebook");
                    social.TwitterUrl = row.Field<string>("TwitterUrl");
                    social.IsTwitter = row.Field<bool>("IsTwitter");
                    social.YoutubeUrl = row.Field<string>("YoutubeUrl");
                    social.IsYoutube = row.Field<bool>("IsYoutube");
                    social.LinkedinUrl = row.Field<string>("LinkedinUrl");
                    social.IsLinkedin = row.Field<bool>("IsLinkedin");
                    social.InstagramUrl = row.Field<string>("InstagramUrl");
                    social.IsInstagram = row.Field<bool>("IsInstagram");
                }
                contentList.Add(social);
            }
            return contentList;
        }
        public static bool UpdateSocialMedia(string facebookUrl, bool isFacebook, string twitterUrl, bool isTwitter, string youtubeUrl, bool isYoutube, string linkedinUrl, bool isLinkedin, string instagramUrl, bool isInstagram)
        {
            return DAL.EditSocialMedia(facebookUrl, isFacebook, twitterUrl, isTwitter, youtubeUrl, isYoutube, linkedinUrl, isLinkedin, instagramUrl, isInstagram);
        }
    }
    public partial class DataAccessLayer
    {
        public DataTable GetListSocialMedia()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_SocialMedia_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool EditSocialMedia(string pFacebookUrl, bool pIsFacebook, string pTwitterUrl, bool pIsTwitter, string pYoutubeUrl, bool pIsYoutube, string pLinkedinUrl, bool pIsLinkedin, string pInstagramUrl, bool pIsInstagram)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_SocialMedia_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pFacebookUrl, pIsFacebook, pTwitterUrl, pIsTwitter, pYoutubeUrl, pIsYoutube, pLinkedinUrl, pIsLinkedin, pInstagramUrl, pIsInstagram });
        }

    }
}