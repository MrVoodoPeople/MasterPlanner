   M S B A M L   `   `   `  ����H  DMasterPlanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:MasterPlanner.ViewMasterPlanner.View  5   	   #    MasterPlanner.View.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:MasterPlanner.View     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6%   .+�5      &   System.Windows.Controls.Calendar  -   5          Name$ 	calendar1��5      6*   �� 5      -   5         ��Name$ 	dateLabel��5      5      �� 5   
   -   5         ��Name$ listView��5      $��1��6C   ��6%   �� *Items+ �� View 5      �� 5      .P�5      �� $
��Date��6%    �� DisplayMemberBinding 63   �� *Date+�� 5      $��DateEnd��6%    66   �� *	DateEnd+�� 5      $��Notes��6%    �� Width$
 Auto��6[    64   �� *Notes+6[   5      5!      �� 5#   
   $��2��6   $��0��6%   .+�5$      �� -   5        $��Добавить��5$      6(   �� 5%      -   5        $��Удалить��5%      6)   5'      5(      5)      