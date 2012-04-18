Option Strict On
'Own release in my Github ... written for Youtube Downloader V2 - also avaible on Github.
Public Class YoutubeVideo
    Dim url As String
    Dim ytitle As String
    Dim inhaa As String
    Sub New(ByVal url As String)
        Dim wc1 As New Net.WebClient
        Dim inhalt As String
        Try
            inhalt = wc1.DownloadString(url)
        Catch ex As Exception
            Throw ex
        End Try
        inhaa = inhalt
        Try
            inhalt = inhalt.Substring(inhalt.IndexOf("<meta property=""og:title"" content=") + 35)
            inhalt = inhalt.Substring(0, inhalt.IndexOf(">") - 1)
        Catch ex As Exception
            Throw ex
        End Try
        ytitle = inhalt
    End Sub

    ReadOnly Property getTitle() As String
        Get
            Return ytitle
        End Get
    End Property

    ReadOnly Property getDescription() As String
        Get
            Dim rDest As String = inhaa
            rDest = rDest.Substring(rDest.IndexOf("<p id=""eow-description"" >") + 25)
            rDest = rDest.Substring(0, rDest.IndexOf("</p>"))
            Return rDest
        End Get
    End Property

    ReadOnly Property getAuthor() As String
        Get
            Dim rAuthor As String = inhaa
            rAuthor = rAuthor.Substring(rAuthor.IndexOf("rel=""author"" dir=""ltr"">") + 23)
            rAuthor = rAuthor.Substring(0, rAuthor.IndexOf("</a>"))
            Return rAuthor
        End Get
    End Property

    ReadOnly Property getUploaddate() As String
        Get
            Dim rUpdate As String = inhaa
            rUpdate = rUpdate.Substring(rUpdate.IndexOf("class=""watch-video-date"" >") + 26)
            rUpdate = rUpdate.Substring(0, rUpdate.IndexOf("</span>"))
            Return rUpdate
        End Get
    End Property

    ReadOnly Property getThumbnailURL() As String
        Get
            Dim rThumb As String = inhaa
            rThumb = rThumb.Substring(rThumb.IndexOf("<link itemprop=""thumbnailUrl"" href=""") + 36)
            rThumb = rThumb.Substring(0, rThumb.IndexOf(""">"))
            Return rThumb
        End Get
    End Property

    ReadOnly Property getViews() As String
        Get
            Dim rViews As String = inhaa
            rViews = rViews.Substring(rViews.IndexOf("<span class=""watch-view-count"">") + 46)
            rViews = rViews.Substring(0, rViews.IndexOf("</strong>"))
            Return rViews
        End Get
    End Property

    ReadOnly Property getLikes() As Integer
        Get
            Dim rLikes As String = inhaa
            rLikes = rLikes.Substring(rLikes.IndexOf("class=""likes"">") + 14)
            rLikes = rLikes.Substring(0, rLikes.IndexOf("</span>"))
            Return CInt(rLikes)
        End Get
    End Property

    ReadOnly Property getDislikes() As Integer
        Get
            Dim rDisLikes As String = inhaa
            rDisLikes = rDisLikes.Substring(rDisLikes.IndexOf("class=""dislikes"">") + 17)
            rDisLikes = rDisLikes.Substring(0, rDisLikes.IndexOf("</span>"))
            Return CInt(rDisLikes)
        End Get
    End Property

    ReadOnly Property getDownloadlink As String
        Get
            Dim pos1 As Integer = inhaa.IndexOf("url_encoded_fmt_stream_map=url%3D")
            Dim link1 As String = inhaa.Substring(pos1 + 33)
            Dim pos2 As Integer = link1.IndexOf("%26fallback_host")
            link1 = link1.Substring(0, pos2)
            link1 = link1.Replace("%3F", "?").Replace("%3D", "=").Replace("%26", "&").Replace("%25", "%").Replace("%2C", ",").Replace("%25", "%").Replace("%3A", ":").Replace("%2F", "/")
            link1 = link1.Replace("%3F", "?").Replace("%3D", "=").Replace("%26", "&").Replace("%25", "%").Replace("%2C", ",").Replace("%25", "%").Replace("%3A", ":").Replace("%2F", "/")
            Return link1
        End Get
    End Property
End Class
