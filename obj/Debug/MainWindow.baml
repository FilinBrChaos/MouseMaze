   M S B A M L   `   `   `  ����D  @MouseMaze, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MouseMaze	MouseMaze  5   	       MouseMaze.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:MouseMaze     5   	     � WindowStyle$
  None=�5   	   $	��800��5	   	   $	��800��6   $	��800��5
   	   $	��800��6    � AllowsTransparency . 
������  � 
ResizeMode$ CanResizeWithGrip=�5   	    � ShowInTaskbar . .��5      � -   5        $��MainGrid��5         WindowBackgroundColor#��[  5       J� Effect 5   
   -  'System.Windows.Media.Effects.BlurEffect  5      -   5        Blur��5      .+�5   
   � $��10q�6   
������ .+�5      �� -   5        $
��Cont��5      $��Center=�5      $��Center=�5      $	��600��5      $	��600��5      5      � 5   
   $��80��6   $��40��6   $	��Top=�5      $��Right=�5       � ColumnDefinitions 5      �� 5       �� 5!      .+�5#      �� -   5        $��	btnResize��5$        WindOperationBtn#��[ 5#      |�.  $��5q�5'      �� 5(      -   5        $��btnClose��5*      $��1��5(      #��[ 5)      |�.  $��5q�5-      5.      � 5/   
   $��50��6   $��50��6   $	��Top=�50      $
��Left=�51      $��10q�52      .+�53      �� -   5        $��btnPause��54      $��0��53      |�.  #��[ 57      58      59      5:      