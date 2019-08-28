using FlickrNet;
using FlickrNet.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Site.Helpers
{
    public class CompanyNameFlickr : Flickr
    {

        public CompanyNameFlickr()
            : base(WebConfigurationManager.AppSettings["FlickrAPIKey"], WebConfigurationManager.AppSettings["FlickrSecurityShared"])
        {
        }

        public PhotosetPhotoCollection PhotosetsGetPhotos(string photosetId, int page, int perPage)
        {
            try
            {
                return base.PhotosetsGetPhotos(photosetId, page, perPage);
            }
            catch (PhotosetNotFoundException)
            {
                return null;
            }        
        }

    }
}