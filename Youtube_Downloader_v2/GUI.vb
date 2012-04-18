Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Drawing.Drawing2D
Imports System.Text.RegularExpressions
MustInherit Class Theme
    Inherits ContainerControl

#Region " Initialization "

    Protected G As Graphics
    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)
    End Sub

    Private ParentIsForm As Boolean
    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        Dock = DockStyle.Fill
        ParentIsForm = TypeOf Parent Is Form
        If ParentIsForm Then
            If Not _TransparencyKey = Color.Empty Then ParentForm.TransparencyKey = _TransparencyKey
            ParentForm.FormBorderStyle = FormBorderStyle.None
        End If
        MyBase.OnHandleCreated(e)
    End Sub

#End Region

#Region " Sizing and Movement "

    Private _Resizable As Boolean = True
    Property Resizable() As Boolean
        Get
            Return _Resizable
        End Get
        Set(ByVal value As Boolean)
            _Resizable = value
        End Set
    End Property

    Private _MoveHeight As Integer = 24
    Property MoveHeight() As Integer
        Get
            Return _MoveHeight
        End Get
        Set(ByVal v As Integer)
            _MoveHeight = v
            Header = New Rectangle(7, 7, Width - 14, _MoveHeight - 7)
        End Set
    End Property

    Private Flag As IntPtr
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If Not e.Button = MouseButtons.Left Then Return
        If ParentIsForm Then If ParentForm.WindowState = FormWindowState.Maximized Then Return

        If Header.Contains(e.Location) Then
            Flag = New IntPtr(2)
        ElseIf Current.Position = 0 Or Not _Resizable Then
            Return
        Else
            Flag = New IntPtr(Current.Position)
        End If

        Capture = False
        DefWndProc(Message.Create(Parent.Handle, 161, Flag, Nothing))

        MyBase.OnMouseDown(e)
    End Sub

    Private Structure Pointer
        ReadOnly Cursor As Cursor, Position As Byte
        Sub New(ByVal c As Cursor, ByVal p As Byte)
            Cursor = c
            Position = p
        End Sub
    End Structure

    Private F1, F2, F3, F4 As Boolean, PTC As Point
    Private Function GetPointer() As Pointer
        PTC = PointToClient(MousePosition)
        F1 = PTC.X < 7
        F2 = PTC.X > Width - 7
        F3 = PTC.Y < 7
        F4 = PTC.Y > Height - 7

        If F1 And F3 Then Return New Pointer(Cursors.SizeNWSE, 13)
        If F1 And F4 Then Return New Pointer(Cursors.SizeNESW, 16)
        If F2 And F3 Then Return New Pointer(Cursors.SizeNESW, 14)
        If F2 And F4 Then Return New Pointer(Cursors.SizeNWSE, 17)
        If F1 Then Return New Pointer(Cursors.SizeWE, 10)
        If F2 Then Return New Pointer(Cursors.SizeWE, 11)
        If F3 Then Return New Pointer(Cursors.SizeNS, 12)
        If F4 Then Return New Pointer(Cursors.SizeNS, 15)
        Return New Pointer(Cursors.Default, 0)
    End Function

    Private Current, Pending As Pointer
    Private Sub SetCurrent()
        Pending = GetPointer()
        If Current.Position = Pending.Position Then Return
        Current = GetPointer()
        Cursor = Current.Cursor
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If _Resizable Then SetCurrent()
        MyBase.OnMouseMove(e)
    End Sub

    Protected Header As Rectangle
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If Width = 0 OrElse Height = 0 Then Return
        Header = New Rectangle(7, 7, Width - 14, _MoveHeight + 7)
        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub

#End Region

#Region " Convienence "

    MustOverride Sub PaintHook()
    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return
        G = e.Graphics
        PaintHook()
    End Sub

    Private _TransparencyKey As Color
    Property TransparencyKey() As Color
        Get
            Return _TransparencyKey
        End Get
        Set(ByVal v As Color)
            _TransparencyKey = v
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            _Image = value
            Invalidate()
        End Set
    End Property
    ReadOnly Property ImageWidth() As Integer
        Get
            If _Image Is Nothing Then Return 0
            Return _Image.Width
        End Get
    End Property

    Private _Size As Size
    Private _Rectangle As Rectangle
    Private _Gradient As LinearGradientBrush
    Private _Brush As SolidBrush

    Protected Sub DrawCorners(ByVal c As Color, ByVal rect As Rectangle)
        _Brush = New SolidBrush(c)
        G.FillRectangle(_Brush, rect.X, rect.Y, 1, 1)
        G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y, 1, 1)
        G.FillRectangle(_Brush, rect.X, rect.Y + (rect.Height - 1), 1, 1)
        G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), 1, 1)
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal p2 As Pen, ByVal rect As Rectangle)
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3)
    End Sub

    Protected Sub DrawText(ByVal a As HorizontalAlignment, ByVal c As Color, ByVal x As Integer)
        DrawText(a, System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(73, Byte), Integer)), x, 0)
    End Sub
    Protected Sub DrawText(ByVal a As HorizontalAlignment, ByVal c As Color, ByVal x As Integer, ByVal y As Integer)
        If String.IsNullOrEmpty(Text) Then Return
        _Size = G.MeasureString(Text, Font).ToSize
        _Brush = New SolidBrush(c)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(Text, Font, _Brush, 5 + x, _MoveHeight \ 2 - _Size.Height \ 2 + 7 + y)
            Case HorizontalAlignment.Right
                G.DrawString(Text, Font, _Brush, Width - 5 - _Size.Width - x, _MoveHeight \ 2 - _Size.Height \ 2 + 7 + y)
            Case HorizontalAlignment.Center
                G.DrawString(Text, Font, _Brush, Width \ 2 - _Size.Width \ 2, _MoveHeight \ 2 - _Size.Height \ 2 + 7)
        End Select
    End Sub

    Protected Sub DrawIcon(ByVal a As HorizontalAlignment, ByVal x As Integer)
        DrawIcon(a, x, 0)
    End Sub
    Protected Sub DrawIcon(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If _Image Is Nothing Then Return
        Select Case a
            Case HorizontalAlignment.Left
                G.DrawImage(_Image, 5 + x, _MoveHeight \ 2 - _Image.Height \ 2 + 7 + y)
            Case HorizontalAlignment.Right
                G.DrawImage(_Image, Width - 5 - _Image.Width - x, _MoveHeight \ 2 - _Image.Height \ 2 + 7 + y)
            Case HorizontalAlignment.Center
                G.DrawImage(_Image, Width \ 2 - _Image.Width \ 2, _MoveHeight \ 2 - _Image.Height \ 2 + 7)
        End Select
    End Sub

    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        _Rectangle = New Rectangle(x, y, width, height)
        _Gradient = New LinearGradientBrush(_Rectangle, c1, c2, angle)
        G.FillRectangle(_Gradient, _Rectangle)
    End Sub

#End Region

End Class
MustInherit Class ThemeControl
    Inherits Control

#Region " Initialization "

    Protected G As Graphics, B As Bitmap
    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)
        B = New Bitmap(1, 1)
        G = Graphics.FromImage(B)
    End Sub

    Sub AllowTransparent()
        SetStyle(ControlStyles.Opaque, False)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub

#End Region

#Region " Mouse Handling "

    Protected Enum State As Byte
        MouseNone = 0
        MouseOver = 1
        MouseDown = 2
    End Enum

    Protected MouseState As State
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        ChangeMouseState(State.MouseNone)
        MyBase.OnMouseLeave(e)
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        ChangeMouseState(State.MouseOver)
        MyBase.OnMouseEnter(e)
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        ChangeMouseState(State.MouseOver)
        MyBase.OnMouseUp(e)
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then ChangeMouseState(State.MouseDown)
        MyBase.OnMouseDown(e)
    End Sub

    Private Sub ChangeMouseState(ByVal e As State)
        MouseState = e
        Invalidate()
    End Sub

#End Region

#Region " Convienence "

    MustOverride Sub PaintHook()
    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return
        PaintHook()
        e.Graphics.DrawImage(B, 0, 0)
    End Sub

    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If Not Width = 0 AndAlso Not Height = 0 Then
            B = New Bitmap(Width, Height)
            G = Graphics.FromImage(B)
            Invalidate()
        End If
        MyBase.OnSizeChanged(e)
    End Sub

    Private _NoRounding As Boolean
    Property NoRounding() As Boolean
        Get
            Return _NoRounding
        End Get
        Set(ByVal v As Boolean)
            _NoRounding = v
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            _Image = value
            Invalidate()
        End Set
    End Property
    ReadOnly Property ImageWidth() As Integer
        Get
            If _Image Is Nothing Then Return 0
            Return _Image.Width
        End Get
    End Property
    ReadOnly Property ImageTop() As Integer
        Get
            If _Image Is Nothing Then Return 0
            Return Height \ 2 - _Image.Height \ 2
        End Get
    End Property

    Private _Size As Size
    Private _Rectangle As Rectangle
    Private _Gradient As LinearGradientBrush
    Private _Brush As SolidBrush

    Protected Sub DrawCorners(ByVal c As Color, ByVal rect As Rectangle)
        If _NoRounding Then Return

        B.SetPixel(rect.X, rect.Y, c)
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c)
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c)
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c)
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal p2 As Pen, ByVal rect As Rectangle)
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3)
    End Sub

    Protected Sub DrawText(ByVal a As HorizontalAlignment, ByVal c As Color, ByVal x As Integer)
        DrawText(a, System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(73, Byte), Integer)), x, 0)
    End Sub
    Protected Sub DrawText(ByVal a As HorizontalAlignment, ByVal c As Color, ByVal x As Integer, ByVal y As Integer)
        If String.IsNullOrEmpty(Text) Then Return
        _Size = G.MeasureString(Text, Font).ToSize
        _Brush = New SolidBrush(c)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(Text, Font, _Brush, 5 + x, Height \ 2 - _Size.Height \ 2 + y)
            Case HorizontalAlignment.Right
                G.DrawString(Text, Font, _Brush, Width - 5 - _Size.Width - x, Height \ 2 - _Size.Height \ 2 + y)
            Case HorizontalAlignment.Center
                G.DrawString(Text, Font, _Brush, Width \ 2 - _Size.Width \ 2, Height \ 2 - _Size.Height \ 2)
        End Select
    End Sub

    Protected Sub DrawIcon(ByVal a As HorizontalAlignment, ByVal x As Integer)
        DrawIcon(a, x, 0)
    End Sub
    Protected Sub DrawIcon(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If _Image Is Nothing Then Return
        Select Case a
            Case HorizontalAlignment.Left
                G.DrawImage(_Image, 5 + x, Height \ 2 - _Image.Height \ 2 + y)
            Case HorizontalAlignment.Right
                G.DrawImage(_Image, Width - 5 - _Image.Width - x, Height \ 2 - _Image.Height \ 2 + y)
            Case HorizontalAlignment.Center
                G.DrawImage(_Image, Width \ 2 - _Image.Width \ 2, Height \ 2 - _Image.Height \ 2)
        End Select
    End Sub

    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        _Rectangle = New Rectangle(x, y, width, height)
        _Gradient = New LinearGradientBrush(_Rectangle, c1, c2, angle)
        G.FillRectangle(_Gradient, _Rectangle)
    End Sub
#End Region

End Class
Class NTheme
    Inherits Theme
    Sub New()
        MoveHeight = 22

        C1 = Color.FromArgb(235, 235, 235)
        P1 = New Pen(Color.FromArgb(150, 150, 150))

        BackColor = Color.White
        ForeColor = Color.FromArgb(110, 110, 110)
        TransparencyKey = Color.Fuchsia
    End Sub

    Private C1 As Color
    Private P1 As Pen

    Overrides Sub PaintHook()
        G.Clear(C1)

        DrawGradient(Color.White, C1, 0, 0, Width, 26, 90S)

        DrawBorders(P1, Pens.White, ClientRectangle)
        DrawCorners(Color.Fuchsia, ClientRectangle)

        G.FillRectangle(Brushes.White, New Rectangle(6, 32, Width - 12, Height - 38))

        DrawText(HorizontalAlignment.Left, ForeColor, ImageWidth)
        DrawIcon(HorizontalAlignment.Left, 0)

        DrawBorders(Pens.White, P1, New Rectangle(6, 32, Width - 12, Height - 38))

        DrawCorners(C1, New Rectangle(6, 32, Width - 12, Height - 38))
        DrawCorners(Color.White, New Rectangle(7, 33, Width - 14, Height - 40))
    End Sub

End Class
Class NButton
    Inherits ThemeControl
    Sub New()
        AllowTransparent()
        C1 = Color.FromArgb(220, 220, 220)
        C2 = Color.FromArgb(250, 250, 250)

        C5 = Color.FromArgb(180, 217, 240)
        C6 = Color.FromArgb(114, 176, 220)

        P1 = New Pen(Color.FromArgb(180, 180, 180))
    End Sub

    Private C1, C2, C5, C6 As Color
    Private P1 As Pen

    Overrides Sub PaintHook()
        If MouseState = State.MouseDown Then
            DrawGradient(C6, C5, 0, 0, Width, Height, 90S)
        ElseIf MouseState = State.MouseOver Then
            DrawGradient(C5, C6, 0, 0, Width, Height, 90S)
        Else
            DrawGradient(C2, C1, 0, 0, Width, Height, 90S)
            G.DrawLine(Pens.White, 0, 1, Width, 1)
            DrawGradient(Color.White, Color.Transparent, 1, 0, 1, Height, 90S)
        End If

        DrawText(HorizontalAlignment.Center, ForeColor, ImageWidth)
        DrawIcon(HorizontalAlignment.Left, 0)

        G.DrawRectangle(P1, 0, 0, Width - 1, Height - 1)

        DrawCorners(BackColor, ClientRectangle)
    End Sub

End Class
Class GTheme
    Inherits Theme

    Sub New()
        MoveHeight = 17

        C1 = Color.FromArgb(41, 41, 41)
        C2 = Color.FromArgb(25, 25, 25)

        P1 = New Pen(Color.FromArgb(58, 58, 58))
        P2 = New Pen(C2)

        BackColor = C1
        ForeColor = Color.FromArgb(100, 100, 100)
        TransparencyKey = Color.Fuchsia
    End Sub

    Private C1, C2 As Color
    Private P1, P2 As Pen

    Overrides Sub PaintHook()
        G.Clear(C1)

        DrawGradient(C2, C1, 0, 0, Width, 28, 90S)

        G.DrawLine(P2, 0, 28, Width, 28)
        G.DrawLine(P1, 0, 29, Width, 29)

        DrawText(HorizontalAlignment.Left, ForeColor, ImageWidth)
        DrawIcon(HorizontalAlignment.Left, 0)

        DrawBorders(Pens.Black, P1, ClientRectangle)
        DrawCorners(Color.Fuchsia, ClientRectangle)
    End Sub

End Class
Class GButton
    Inherits ThemeControl

    Private P1, P2 As Pen
    Private C1, C2 As Color

    Sub New()
        P1 = New Pen(Color.FromArgb(25, 25, 25))
        P2 = New Pen(Color.FromArgb(11, Color.White))

        C1 = Color.FromArgb(41, 41, 41)
        C2 = Color.FromArgb(51, 51, 51)
    End Sub

    Overrides Sub PaintHook()

        If MouseState = State.MouseDown Then
            DrawGradient(C1, C2, 0, 0, Width, Height, 90S)
        Else
            DrawGradient(C2, C1, 0, 0, Width, Height, 90S)
        End If

        DrawText(HorizontalAlignment.Center, ForeColor, 0)
        DrawIcon(HorizontalAlignment.Left, 0)

        DrawBorders(P1, P2, ClientRectangle)
        DrawCorners(BackColor, ClientRectangle)
    End Sub

End Class
Class RTheme
    Inherits Theme

    Sub New()
        MoveHeight = 14

        C1 = Color.FromArgb(40, 40, 40)
        C2 = Color.FromArgb(30, 30, 30)

        B1 = New SolidBrush(Color.FromArgb(12, Color.White))
        B2 = New SolidBrush(Color.FromArgb(28, 28, 28))

        P1 = New Pen(Color.FromArgb(90, 90, 90))
        P2 = New Pen(Color.FromArgb(70, 70, 70))

        CreateTile()

        BackColor = B2.Color
        ForeColor = Color.White
        TransparencyKey = Color.Fuchsia
    End Sub

    Private Tile As TextureBrush
    Private Sub CreateTile()
        Dim T As New Bitmap(4, 4)
        For I As Byte = 0 To 3
            T.SetPixel(3 - I, I, C1)
        Next
        Tile = New TextureBrush(T)
    End Sub

    Private C1, C2 As Color
    Private B1, B2 As SolidBrush
    Private P1, P2 As Pen

    Overrides Sub PaintHook()
        G.Clear(C1)

        DrawGradient(C2, C1, 0, 0, Width, 24, 90S)
        G.FillRectangle(Tile, 0, 0, Width, 28)

        G.FillRectangle(B1, 0, 0, Width, 12)
        G.FillRectangle(B2, 6, 26, Width - 12, Height - 32)

        DrawText(HorizontalAlignment.Left, Color.White, ImageWidth)
        DrawIcon(HorizontalAlignment.Left, 0)

        DrawBorders(Pens.Black, P1, ClientRectangle)
        DrawBorders(P2, Pens.Black, New Rectangle(6, 26, Width - 12, Height - 32))

        DrawCorners(Color.Fuchsia, ClientRectangle)
    End Sub

End Class
Class RButton
    Inherits ThemeControl

    Sub New()
        C1 = Color.FromArgb(40, 40, 40)
        C2 = Color.FromArgb(30, 30, 30)
        C3 = Color.FromArgb(20, Color.White)
        C4 = Color.FromArgb(35, 255, 255, 255)

        B1 = New SolidBrush(Color.FromArgb(10, Color.White))
        P1 = New Pen(Color.FromArgb(65, 65, 65))

        CreateTile()
    End Sub

    Private Tile As TextureBrush
    Private Sub CreateTile()
        Dim T As New Bitmap(4, 4)
        For I As Byte = 0 To 3
            T.SetPixel(3 - I, I, C1)
        Next
        Tile = New TextureBrush(T)
    End Sub

    Private I1 As Integer
    Private C1, C2, C3, C4 As Color
    Private B1 As SolidBrush
    Private P1 As Pen

    Overrides Sub PaintHook()
        G.Clear(C1)

        DrawGradient(C2, C1, 0, 0, Width, CInt(Height * 0.85), 90S)
        G.FillRectangle(Tile, ClientRectangle)

        I1 = CInt(Height * 0.45)

        Select Case MouseState
            Case State.MouseNone
                DrawGradient(C3, Color.Transparent, 0, 0, Width, I1, 90S)
            Case State.MouseOver
                DrawGradient(C3, Color.Transparent, 0, 0, Width, I1, 90S)
                G.FillRectangle(B1, ClientRectangle)
        End Select

        DrawText(HorizontalAlignment.Center, Color.White, ImageWidth)
        DrawIcon(HorizontalAlignment.Left, 0)

        DrawBorders(Pens.Black, P1, ClientRectangle)

        DrawGradient(Color.Transparent, C4, 1, 0, 1, Height \ 2, 90S)
        DrawGradient(Color.Transparent, C4, Width - 2, 0, 1, Height \ 2, 90S)
    End Sub
End Class
<ProvideProperty("Frame", GetType(Control))> _
Class Rotator
    Inherits Component
    Implements IExtenderProvider

    Private _CurrentFrame As Byte
    Property CurrentFrame() As Byte
        Get
            Return _CurrentFrame
        End Get
        Set(ByVal v As Byte)
            _CurrentFrame = v

            Dim Current As Byte
            For Each c As Control In children.Keys
                Current = CByte(children(c))
                c.Visible = Current = _CurrentFrame OrElse Current = 255
            Next
        End Set
    End Property

    Private children As New Hashtable
    Private Function CanExtend(ByVal c As Object) As Boolean Implements IExtenderProvider.CanExtend
        If Not TypeOf c Is Control OrElse TypeOf c Is Form Then Return False

        If Not children.Contains(c) Then
            children.Add(c, 0)
            AddHandler DirectCast(c, Control).HandleCreated, AddressOf HandleCreated
        End If

        Return True
    End Function

    Private Sub HandleCreated(ByVal s As Object, ByVal e As EventArgs)
        children(DirectCast(s, Control)) = _CurrentFrame
    End Sub

    Function GetFrame(ByVal c As Control) As Byte
        Return CByte(children(c))
    End Function
    Sub SetFrame(ByVal c As Control, ByVal v As Byte)
        children(c) = v
        c.Visible = v = _CurrentFrame OrElse v = 255
    End Sub
End Class
Class Seperator
    Inherits ThemeControl

    Sub New()
        AllowTransparent()
    End Sub

    Private _Direction As Orientation
    Property Direction() As Orientation
        Get
            Return _Direction
        End Get
        Set(ByVal value As Orientation)
            _Direction = value
            Invalidate()
        End Set
    End Property

    Private _Color1 As Color = Color.FromArgb(90, Color.Black)
    Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
            Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(14, Color.White)
    Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
            Invalidate()
        End Set
    End Property

    Overrides Sub PaintHook()
        If Not BackColor = Color.Transparent Then G.Clear(BackColor)

        If _Direction = Orientation.Horizontal Then
            G.DrawLine(New Pen(_Color1), 0, Height \ 2, Width, Height \ 2)
            G.DrawLine(New Pen(_Color2), 0, Height \ 2 + 1, Width, Height \ 2 + 1)
        Else
            G.DrawLine(New Pen(_Color1), Width \ 2, 0, Width \ 2, Height)
            G.DrawLine(New Pen(_Color2), Width \ 2 + 1, 0, Width \ 2 + 1, Height)
        End If
    End Sub

End Class
Class ButtonPane
    Inherits ThemeControl

    Sub New()
        AllowTransparent()

        _InnerBorder = New Pen(Color.FromArgb(50, Color.White))
        _Border1 = New Pen(Color.FromArgb(190, 215, 250))
        _Border2 = New Pen(Color.FromArgb(175, 200, 230))

        _Font1 = Font
        _Font2 = New Font(Font.Name, Font.Size - 1)

        _TextColor1 = New SolidBrush(Color.Blue)
        _TextColor2 = New SolidBrush(Color.Black)
        GO = Graphics.FromHwndInternal(Handle)
        UpdateSize()
    End Sub

    Private GO As Graphics
    Private TextSize1, TextSize2 As Size
    Private Sub UpdateSize()
        TextSize1 = GO.MeasureString(_Text1, _Font1).ToSize
        TextSize2 = GO.MeasureString(_Text2, _Font2).ToSize
        Invalidate()
    End Sub

    Private _Text1 As String = "Name"
    Property Text1() As String
        Get
            Return _Text1
        End Get
        Set(ByVal v As String)
            _Text1 = v
            UpdateSize()
        End Set
    End Property
    Private _Text2 As String = "Description"
    Property Text2() As String
        Get
            Return _Text2
        End Get
        Set(ByVal v As String)
            _Text2 = v
            UpdateSize()
        End Set
    End Property

    Private _Font1 As Font
    Property Font1() As Font
        Get
            Return _Font1
        End Get
        Set(ByVal v As Font)
            _Font1 = v
            UpdateSize()
        End Set
    End Property
    Private _Font2 As Font
    Property Font2() As Font
        Get
            Return _Font2
        End Get
        Set(ByVal v As Font)
            _Font2 = v
            UpdateSize()
        End Set
    End Property

    Private _TextColor1 As SolidBrush
    Property TextColor1() As Color
        Get
            Return _TextColor1.Color
        End Get
        Set(ByVal v As Color)
            _TextColor1 = New SolidBrush(v)
            Invalidate()
        End Set
    End Property
    Private _TextColor2 As SolidBrush
    Property TextColor2() As Color
        Get
            Return _TextColor2.Color
        End Get
        Set(ByVal v As Color)
            _TextColor2 = New SolidBrush(v)
            Invalidate()
        End Set
    End Property

    Private _InnerBorder As Pen
    Property InnerBorder() As Color
        Get
            Return _InnerBorder.Color
        End Get
        Set(ByVal v As Color)
            _InnerBorder = New Pen(v)
            Invalidate()
        End Set
    End Property
    Private _Border1 As Pen
    Property Border1() As Color
        Get
            Return _Border1.Color
        End Get
        Set(ByVal v As Color)
            _Border1 = New Pen(v)
            Invalidate()
        End Set
    End Property
    Private _Border2 As Pen
    Property Border2() As Color
        Get
            Return _Border2.Color
        End Get
        Set(ByVal v As Color)
            _Border2 = New Pen(v)
            Invalidate()
        End Set
    End Property

    Private _NoGradient As Boolean
    Property NoGradient() As Boolean
        Get
            Return _NoGradient
        End Get
        Set(ByVal v As Boolean)
            _NoGradient = v
            Invalidate()
        End Set
    End Property

    Private _Color1A As Color = Color.White
    Property Color1A() As Color
        Get
            Return _Color1A
        End Get
        Set(ByVal v As Color)
            _Color1A = v
            Invalidate()
        End Set
    End Property
    Private _Color1B As Color = Color.FromArgb(245, 245, 245)
    Property Color1B() As Color
        Get
            Return _Color1B
        End Get
        Set(ByVal v As Color)
            _Color1B = v
            Invalidate()
        End Set
    End Property

    Private _Color2A As Color = Color.FromArgb(205, 230, 255)
    Property Color2A() As Color
        Get
            Return _Color2A
        End Get
        Set(ByVal v As Color)
            _Color2A = v
            Invalidate()
        End Set
    End Property
    Private _Color2B As Color = Color.FromArgb(195, 220, 255)
    Property Color2B() As Color
        Get
            Return _Color2B
        End Get
        Set(ByVal v As Color)
            _Color2B = v
            Invalidate()
        End Set
    End Property

    Private _Selection As Boolean
    Property Selection() As Boolean
        Get
            Return _Selection
        End Get
        Set(ByVal v As Boolean)
            _Selection = v
            Invalidate()
        End Set
    End Property

    Private _SelectionToggle As Boolean
    Property SelectionToggle() As Boolean
        Get
            Return _SelectionToggle
        End Get
        Set(ByVal v As Boolean)
            _SelectionToggle = v
        End Set
    End Property
    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        If _SelectionToggle Then Selection = Not _Selection
        Invalidate()
        MyBase.OnClick(e)
    End Sub

    Private Push As Byte
    Private ImageOS, StringTO As Integer
    Overrides Sub PaintHook()
        If Not BackColor = Color.Transparent Then G.Clear(BackColor)

        Push = 0
        Select Case MouseState
            Case State.MouseOver
                If _Selection Then DrawMouseDown() Else DrawMouseOver()
            Case State.MouseDown
                Push = 1
                DrawMouseDown()
            Case State.MouseNone
                If _Selection Then DrawMouseOver()
        End Select

        If ImageWidth = 0 Then ImageOS = 0 Else ImageOS = 10
        StringTO = Height \ 2 - TextSize1.Height \ 2 - TextSize2.Height \ 2

        G.DrawString(_Text1, _Font1, _TextColor1, ImageWidth + ImageOS + 10 + Push, StringTO + Push)
        DrawDescription()

        DrawIcon(HorizontalAlignment.Left, 5 + Push, Push)
    End Sub

    Private Sub DrawDescription()
        If String.IsNullOrEmpty(Text1) Then
            G.DrawString(_Text2, _Font2, _TextColor2, ImageWidth + ImageOS + 10 + Push, StringTO + TextSize1.Height + Push)
        Else
            G.DrawString(_Text2, _Font2, _TextColor2, ImageWidth + ImageOS + 10 + Push, StringTO + TextSize1.Height + Push)
        End If
    End Sub

    Private Sub DrawMouseOver()
        If Not _NoGradient Then DrawGradient(_Color1A, _Color1B, 0, 0, Width, Height, 90S)
        DrawBorders(_Border1, _InnerBorder, ClientRectangle)
        DrawCorners(BackColor, ClientRectangle)
    End Sub
    Private Sub DrawMouseDown()
        If Not _NoGradient Then DrawGradient(_Color2A, _Color2B, 0, 0, Width, Height, 90S)
        DrawBorders(_Border2, _InnerBorder, ClientRectangle)
        DrawCorners(BackColor, ClientRectangle)
    End Sub
End Class
Class GPBOX
    Inherits ThemeControl

    Sub New()
        AllowTransparent()
        _Border1 = New Pen(Color.FromArgb(90, Color.Black))
        _Border2 = New Pen(Color.FromArgb(14, Color.White))
    End Sub

    Private _Border1 As Pen
    Property Border1() As Color
        Get
            Return _Border1.Color
        End Get
        Set(ByVal v As Color)
            _Border1 = New Pen(v)
            Invalidate()
        End Set
    End Property

    Private _Border2 As Pen
    Property Border2() As Color
        Get
            Return _Border2.Color
        End Get
        Set(ByVal v As Color)
            _Border2 = New Pen(v)
            Invalidate()
        End Set
    End Property

    Private _FillColor As Color = Color.Transparent
    Property FillColor() As Color
        Get
            Return _FillColor
        End Get
        Set(ByVal v As Color)
            _FillColor = v
            Invalidate()
        End Set
    End Property

    Overrides Sub PaintHook()
        G.Clear(_FillColor)
        DrawBorders(_Border1, _Border2, ClientRectangle)
        DrawCorners(BackColor, ClientRectangle)
    End Sub

End Class
#Region " Public Class ProgressbarWithPercentage "
''' <summary>Component that extends the native .net progressbar with percentage properties and the ability to overlay the percentage</summary>
''' <remarks>Component ProgressbarWithPercentage v1.0.10, by De Dauw Jeroen - November 2009</remarks>
<DesignTimeVisible(True), DefaultProperty("Value"), DefaultEvent("ValueChanged"), _
Description("Component that extends the native .net progressbar with percentage properties and the ability to overlay the percentage")> _
Public Class ProgressbarWithPercentage
    Inherits System.Windows.Forms.ProgressBar

#Region "Events"
    ''' <summary>Occurs when the value of the progress bar is changed</summary>
    <Category("Property Changed")> _
    Public Event ValueChanged As EventHandler
    ''' <summary>Occurs when the amount of decimals to be displayed in the percentage is changed</summary>
    <Category("Property Changed")> _
    Public Event PercentageDecimalsChanged As EventHandler
    ''' <summary>Occurs when the visibility of the percentage text is changed</summary>
    <Category("Property Changed")> _
    Public Event PercentageVisibleChanged As EventHandler
    ''' <summary>Occurs when the automatic updating of the percentage is turned on or off</summary>
    <Category("Property Changed")> _
    Public Event AutoUpdatePercentageChanged As EventHandler
    ''' <summary>Occurs when the OverlayTextColor property is changed</summary>
    <Category("Property Changed")> _
    Public Event OverlayTextColorChanged As EventHandler
    ''' <summary>Occurs when the TextColor property is changed</summary>
    <Category("Property Changed")> _
    Public Event TextColorChanged As EventHandler
    ''' <summary>Occurs when the align of the percentage text is changed</summary>
    <Category("Property Changed")> _
    Public Event PercentageAlignChanged As EventHandler
    ''' <summary>Occurs when the display format is changed</summary>
    <Category("Property Changed")> _
    Public Event DisplayFormatChanged As EventHandler
    ''' <summary>Occurs when the padding of the percentage text is changed</summary>
    <Category("Property Changed")> _
    Public Shadows Event PaddingChanged As EventHandler
#End Region

#Region "Fields"
    Private Const WM_PAINT As Int32 = &HF 'hex for 15

    Private m_auto_update, m_p_visible As Boolean
    Private m_displayFormat As String
    Private m_decimals As Int32
    Private m_p_align As ContentAlignment
    Private m_graphics As Graphics
    Private m_textColor, m_overlayTextColor As Color
    Private m_drawingRectangle As RectangleF
    Private m_strFormat As New StringFormat
#End Region

#Region "Public methods"
    ''' <summary>Create a new instance of a ProgressbarWithPercentage</summary>
    Public Sub New()
        ' Initialize the base class
        MyBase.New()

        ' Set the default values of the properties
        Me.DisplayFormat = "{0}%"
        Me.AutoUpdatePercentage = True
        Me.PercentageVisible = True
        Me.PercentageDecimals = 0
        Me.PercentageAlign = ContentAlignment.MiddleCenter
        Me.TextColor = Color.DimGray
        Me.OverlayTextColor = Color.Black
        Me.Style = ProgressBarStyle.Continuous

        ' Calculate the initial gfx related values
        setGfx()
        setStringFormat()
        setDrawingRectangle()
    End Sub

    ''' <summary>Advances the current possition of the progressbar by the amount of the Step property</summary>
    Public Shadows Sub PerformStep()
        MyBase.PerformStep()
        If Me.PercentageVisible And Me.AutoUpdatePercentage Then Me.ShowPercentage()
    End Sub

    ''' <summary>Show the current percentage as text</summary>
    Public Sub ShowPercentage()
        Me.ShowText(String.Format(Me.DisplayFormat, Math.Round(Me.Percentage, Me.PercentageDecimals).ToString))
    End Sub

    ''' <summary>Display a string on the progressbar</summary>
    ''' <param name="text">Required. String. The string to display</param>
    Public Sub ShowText(ByVal text As String)
        ' If the style is not marquee, regions will be used to allow the overlay color to be used
        If Me.Style <> ProgressBarStyle.Marquee Then
            ' Determine the areas for the ForeColor and OverlayColor
            Dim r1 As RectangleF = Me.ClientRectangle
            r1.Width = CSng(r1.Width * Me.Value / Me.Maximum)
            Dim reg1 As New Region(r1)
            Dim reg2 As New Region(Me.ClientRectangle)
            reg2.Exclude(reg1)

            ' Draw the string with the correct color depending on the region
            Me.Graphics.Clip = reg1
            Me.Graphics.DrawString(text, Me.Font, New SolidBrush(Me.OverlayTextColor), Me.DrawingRectangle, m_strFormat)
            Me.Graphics.Clip = reg2
            Me.Graphics.DrawString(text, Me.Font, New SolidBrush(Me.TextColor), Me.DrawingRectangle, m_strFormat)

            ' Dispose the regions
            reg1.Dispose()
            reg2.Dispose()
        Else
            ' Draw the string
            Me.Graphics.DrawString(text, Me.Font, New SolidBrush(Me.TextColor), Me.DrawingRectangle, m_strFormat)
        End If
    End Sub
#End Region

#Region "Protected methods"
    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        MyBase.OnHandleCreated(e)
        Me.Graphics = Graphics.FromHwnd(Me.Handle)
    End Sub

    Protected Overrides Sub OnHandleDestroyed(ByVal e As System.EventArgs)
        Me.Graphics.Dispose()
        MyBase.OnHandleDestroyed(e)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
        If m.Msg = WM_PAINT And Me.PercentageVisible And Me.AutoUpdatePercentage Then ShowPercentage()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.AutoUpdatePercentage = False
        If disposing Then
            Me.Graphics.Dispose()
            m_strFormat.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
#End Region

#Region "Private methods"
    Private Sub ProgressbarWithPercentage_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        setDrawingRectangle()
        setGfx()
    End Sub

    Private Sub ProgressbarWithPercentage_PaddingChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.PaddingChanged
        setDrawingRectangle()
    End Sub

    Private Sub setGfx()
        ' Set the graphics object
        Me.Graphics = Me.CreateGraphics
    End Sub

    Private Sub setDrawingRectangle()
        ' Determine the coordinates and size of the drawing rectangle depending on the progress bar size and padding
        Me.DrawingRectangle = New RectangleF(Me.Padding.Left, _
                                   Me.Padding.Top, _
                                   Me.Width - Me.Padding.Left - Me.Padding.Right, _
                                   Me.Height - Me.Padding.Top - Me.Padding.Bottom)
    End Sub

    Private Sub setStringFormat()
        ' Determine the horizontal alignment
        Select Case Me.PercentageAlign
            Case ContentAlignment.BottomCenter, ContentAlignment.BottomLeft, ContentAlignment.BottomRight
                m_strFormat.LineAlignment = StringAlignment.Far
            Case ContentAlignment.MiddleCenter, ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight
                m_strFormat.LineAlignment = StringAlignment.Center
            Case ContentAlignment.TopCenter, ContentAlignment.TopLeft, ContentAlignment.TopRight
                m_strFormat.LineAlignment = StringAlignment.Near
        End Select

        ' Determine the vertical alignment
        Select Case Me.PercentageAlign
            Case ContentAlignment.BottomLeft, ContentAlignment.MiddleLeft, ContentAlignment.TopLeft
                m_strFormat.Alignment = StringAlignment.Near
            Case ContentAlignment.BottomCenter, ContentAlignment.MiddleCenter, ContentAlignment.TopCenter
                m_strFormat.Alignment = StringAlignment.Center
            Case ContentAlignment.BottomRight, ContentAlignment.MiddleRight, ContentAlignment.TopRight
                m_strFormat.Alignment = StringAlignment.Far
        End Select
    End Sub
#End Region

#Region "Properties"

#Region "Appearance"
    <Browsable(True), Category("Appearance"), Description("The value of the progressbar")> _
    Public Shadows Property Value() As Int32
        Get
            Return MyBase.Value
        End Get
        Set(ByVal value As Int32)
            If value <> Me.Value Then
                MyBase.Value = value
                If Me.PercentageVisible And Me.AutoUpdatePercentage Then
                    If Me.VistaVisualStylesEnabled Then
                        Me.Invalidate()
                    Else
                        Me.ShowPercentage()
                    End If
                End If
                RaiseEvent ValueChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), Description("The percentage of the progressbar")> _
    Public Property Percentage() As Double
        Get
            Return Me.Value / Me.Maximum * 100
        End Get
        Set(ByVal value As Double)
            If value >= 0 And value <= 100 Then
                Me.Value = CInt(Me.Maximum * value / 100)
            Else
                Throw New Exception("The percentage can not be needs to be equal or greather then 0 and smaller then or equal to 100")
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), DefaultValue(0), Description("Gets or sets the amount of decimals that will be displayed in the percentage")> _
    Public Overridable Property PercentageDecimals() As Int32
        Get
            Return m_decimals
        End Get
        Set(ByVal value As Int32)
            If value > -1 And value <> Me.PercentageDecimals Then
                m_decimals = value
                RaiseEvent PercentageDecimalsChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), Description("Gets or sets the font of the percentage text")> _
    Public Overridable Overloads Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
        End Set
    End Property

    <Browsable(True), Category("Appearance"), DefaultValue("MiddleCenter"), Description("Gets or sets if the percentage alignment")> _
    Public Overridable Property PercentageAlign() As ContentAlignment
        Get
            Return m_p_align
        End Get
        Set(ByVal value As ContentAlignment)
            If value <> Me.PercentageAlign Then
                m_p_align = value
                setStringFormat()
                RaiseEvent PercentageAlignChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), Description("Gets or sets the color of the percentage text where the progressbar is not indicated")> _
    Public Overridable Property TextColor() As Color
        Get
            Return m_textColor
        End Get
        Set(ByVal value As Color)
            If Me.TextColor <> value Then
                m_textColor = value
                RaiseEvent TextColorChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), Description("Gets or sets the color of the percentage text where the progressbar is indicated")> _
    Public Overridable Property OverlayTextColor() As Color
        Get
            Return m_overlayTextColor
        End Get
        Set(ByVal value As Color)
            If Me.OverlayTextColor <> value Then
                m_overlayTextColor = value
                RaiseEvent OverlayTextColorChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), DefaultValue(True), Description("Gets or sets if the percentage should be visible")> _
    Public Overridable Property PercentageVisible() As Boolean
        Get
            Return m_p_visible
        End Get
        Set(ByVal value As Boolean)
            If value <> Me.PercentageVisible Then
                If Not value Then Me.Graphics.Clear(Me.BackColor)
                m_p_visible = value
                RaiseEvent PercentageVisibleChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), DefaultValue("{0}%"), Description("Gets or sets the display format of the percentage")> _
    Public Property DisplayFormat() As String
        Get
            Return m_displayFormat
        End Get
        Set(ByVal value As String)
            If value <> Me.DisplayFormat Then
                m_displayFormat = value
                Me.Invalidate()
                RaiseEvent DisplayFormatChanged(Me, New EventArgs)
            End If
        End Set
    End Property
#End Region

#Region "Behavior"
    <Browsable(True), Category("Behavior"), DefaultValue(True), Description("Gets or sets if the percentage should be auto updated")> _
    Public Overridable Property AutoUpdatePercentage() As Boolean
        Get
            Return m_auto_update
        End Get
        Set(ByVal value As Boolean)
            If value <> Me.AutoUpdatePercentage Then
                m_auto_update = value
                RaiseEvent AutoUpdatePercentageChanged(Me, New EventArgs)
            End If
        End Set
    End Property
#End Region

#Region "Layout"
    <Browsable(True), Category("Layout"), Description("Gets or sets if the interior spacing of the control")> _
    Public Overridable Overloads Property Padding() As Padding
        Get
            Return MyBase.Padding
        End Get
        Set(ByVal value As Padding)
            MyBase.Padding = value
        End Set
    End Property
#End Region

#Region "Misc"
    Protected Overridable Property Graphics() As Graphics
        Get
            Return m_graphics
        End Get
        Set(ByVal value As Graphics)
            If Me.Graphics IsNot Nothing Then Me.Graphics.Dispose()
            m_graphics = value
        End Set
    End Property

    Private Property DrawingRectangle() As RectangleF
        Get
            Return m_drawingRectangle
        End Get
        Set(ByVal value As RectangleF)
            m_drawingRectangle = value
        End Set
    End Property

    Private ReadOnly Property VistaVisualStylesEnabled() As Boolean
        Get
            Return Environment.OSVersion.Version.Major = 6 And Application.VisualStyleState <> VisualStyles.VisualStyleState.NoneEnabled
        End Get
    End Property
#End Region

#End Region

#Region "Designer"
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        Me.ResumeLayout(False)
    End Sub
#End Region

End Class
#End Region
Namespace ListViewEmbeddedControls
    ''' <summary>
    ''' Zusammenfassung für ListViewEx.
    ''' </summary>
    Public Class ListViewEx
        Inherits ListView
#Region "Interop-Defines"
        <DllImport("user32.dll")> _
        Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wPar As IntPtr, ByVal lPar As IntPtr) As IntPtr
        End Function

        ' ListView messages
        Private Const LVM_FIRST As Integer = &H1000
        Private Const LVM_GETCOLUMNORDERARRAY As Integer = (LVM_FIRST + 59)

        ' Windows Messages
        Private Const WM_PAINT As Integer = &HF
#End Region

        ''' <summary>
        ''' Structure to hold an embedded control's info
        ''' </summary>
        Private Structure EmbeddedControl
            Public Control As Control
            Public Column As Integer
            Public Row As Integer
            Public Dock As DockStyle
            Public Item As ListViewItem
        End Structure

        Private _embeddedControls As New ArrayList()

        Public Sub New()
        End Sub

        ''' <summary>
        ''' Retrieve the order in which columns appear
        ''' </summary>
        ''' <returns>Current display order of column indices</returns>
        Protected Function GetColumnOrder() As Integer()
            Dim lPar As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(Integer)) * Columns.Count)

            Dim res As IntPtr = SendMessage(Handle, LVM_GETCOLUMNORDERARRAY, New IntPtr(Columns.Count), lPar)
            If res.ToInt32() = 0 Then
                ' Something went wrong
                Marshal.FreeHGlobal(lPar)
                Return Nothing
            End If

            Dim order As Integer() = New Integer(Columns.Count - 1) {}
            Marshal.Copy(lPar, order, 0, Columns.Count)

            Marshal.FreeHGlobal(lPar)

            Return order
        End Function

        ''' <summary>
        ''' Retrieve the bounds of a ListViewSubItem
        ''' </summary>
        ''' <param name="Item">The Item containing the SubItem</param>
        ''' <param name="SubItem">Index of the SubItem</param>
        ''' <returns>Subitem's bounds</returns>
        Protected Function GetSubItemBounds(ByVal Item As ListViewItem, ByVal SubItem As Integer) As Rectangle
            Dim subItemRect As Rectangle = Rectangle.Empty

            If Item Is Nothing Then
                Throw New ArgumentNullException("Item")
            End If

            Dim order As Integer() = GetColumnOrder()
            If order Is Nothing Then
                ' No Columns
                Return subItemRect
            End If

            If SubItem >= order.Length Then
                Throw New IndexOutOfRangeException("SubItem " & SubItem & " out of range")
            End If

            ' Retrieve the bounds of the entire ListViewItem (all subitems)
            Dim lviBounds As Rectangle = Item.GetBounds(ItemBoundsPortion.Entire)
            Dim subItemX As Integer = lviBounds.Left

            ' Calculate the X position of the SubItem.
            ' Because the columns can be reordered we have to use Columns[order[i]] instead of Columns[i] !
            Dim col As ColumnHeader
            Dim i As Integer
            For i = 0 To order.Length - 1
                col = Me.Columns(order(i))
                If col.Index = SubItem Then
                    Exit For
                End If
                subItemX += col.Width
            Next

            subItemRect = New Rectangle(subItemX, lviBounds.Top, Me.Columns(order(i)).Width, lviBounds.Height)

            Return subItemRect
        End Function

        ''' <summary>
        ''' Add a control to the ListView
        ''' </summary>
        ''' <param name="c">Control to be added</param>
        ''' <param name="col">Index of column</param>
        ''' <param name="row">Index of row</param>
        Public Sub AddEmbeddedControl(ByVal c As Control, ByVal col As Integer, ByVal row As Integer)
            AddEmbeddedControl(c, col, row, DockStyle.Fill)
            Invalidate()
        End Sub
        ''' <summary>
        ''' Add a control to the ListView
        ''' </summary>
        ''' <param name="c">Control to be added</param>
        ''' <param name="col">Index of column</param>
        ''' <param name="row">Index of row</param>
        ''' <param name="dock">Location and resize behavior of embedded control</param>
        Public Sub AddEmbeddedControl(ByVal c As Control, ByVal col As Integer, ByVal row As Integer, ByVal dock As DockStyle)
            If c Is Nothing Then
                Throw New ArgumentNullException()
            End If
            If col >= Columns.Count OrElse row >= Items.Count Then
                Throw New ArgumentOutOfRangeException()
            End If

            Dim ec As EmbeddedControl
            ec.Control = c
            ec.Column = col
            ec.Row = row
            ec.Dock = dock
            ec.Item = Items(row)

            _embeddedControls.Add(ec)

            ' Add a Click event handler to select the ListView row when an embedded control is clicked
            AddHandler c.Click, New EventHandler(AddressOf _embeddedControl_Click)

            Me.Controls.Add(c)
        End Sub

        ''' <summary>
        ''' Remove a control from the ListView
        ''' </summary>
        ''' <param name="c">Control to be removed</param>
        Public Sub RemoveEmbeddedControl(ByVal c As Control)
            If c Is Nothing Then
                Exit Sub
                'Throw New ArgumentNullException()
            End If

            For i As Integer = 0 To _embeddedControls.Count - 1
                Dim ec As EmbeddedControl = CType(_embeddedControls(i), EmbeddedControl)
                If ec.Control Is c Then
                    RemoveHandler c.Click, New EventHandler(AddressOf _embeddedControl_Click)
                    Me.Controls.Remove(c)
                    _embeddedControls.RemoveAt(i)
                    Return
                End If
            Next
            Throw New Exception("Control not found!")
        End Sub

        ''' <summary>
        ''' Retrieve the control embedded at a given location
        ''' </summary>
        ''' <param name="col">Index of Column</param>
        ''' <param name="row">Index of Row</param>
        ''' <returns>Control found at given location or null if none assigned.</returns>
        Public Function GetEmbeddedControl(ByVal col As Integer, ByVal row As Integer) As Control
            For Each ec As EmbeddedControl In _embeddedControls
                If ec.Row = row AndAlso ec.Column = col Then
                    Return ec.Control
                End If
            Next

            Return Nothing
        End Function

        <DefaultValue(View.LargeIcon)> _
        Public Shadows Property View() As View
            Get
                Return MyBase.View
            End Get
            Set(ByVal value As View)
                ' Embedded controls are rendered only when we're in Details mode
                For Each ec As EmbeddedControl In _embeddedControls
                    ec.Control.Visible = (value = View.Details)
                Next

                MyBase.View = value
            End Set
        End Property

        Protected Overrides Sub WndProc(ByRef m As Message)
            Select Case m.Msg
                Case WM_PAINT
                    If View <> View.Details Then
                        Exit Select
                    End If

                    ' Calculate the position of all embedded controls
                    For Each ec As EmbeddedControl In _embeddedControls
                        Dim rc As Rectangle = Me.GetSubItemBounds(ec.Item, ec.Column)

                        If (Me.HeaderStyle <> ColumnHeaderStyle.None) AndAlso (rc.Top < Me.Font.Height) Then
                            ' Control overlaps ColumnHeader
                            ec.Control.Visible = False
                            Continue For
                        Else
                            ec.Control.Visible = True
                        End If

                        Select Case ec.Dock
                            Case DockStyle.Fill
                                Exit Select
                            Case DockStyle.Top
                                rc.Height = ec.Control.Height
                                Exit Select
                            Case DockStyle.Left
                                rc.Width = ec.Control.Width
                                Exit Select
                            Case DockStyle.Bottom
                                rc.Offset(0, rc.Height - ec.Control.Height)
                                rc.Height = ec.Control.Height
                                Exit Select
                            Case DockStyle.Right
                                rc.Offset(rc.Width - ec.Control.Width, 0)
                                rc.Width = ec.Control.Width
                                Exit Select
                            Case DockStyle.None
                                rc.Size = ec.Control.Size
                                Exit Select
                        End Select

                        ' Set embedded control's bounds
                        ec.Control.Bounds = rc
                    Next
                    Exit Select
            End Select
            MyBase.WndProc(m)
        End Sub

        Private Sub _embeddedControl_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' When a control is clicked the ListViewItem holding it is selected
            For Each ec As EmbeddedControl In _embeddedControls
                If ec.Control Is DirectCast(sender, Control) Then
                    Me.SelectedItems.Clear()
                    ec.Item.Selected = True
                End If
            Next
        End Sub
    End Class
End Namespace
