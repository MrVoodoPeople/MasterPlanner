   M S B A M L   `   `   `  ����H  DMasterPlanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:MasterPlanner.ViewMasterPlanner.View  5   	   '    !MasterPlanner.View.AddTextAndDate   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:MasterPlanner.View     5   	     � Title$  AddTextAndDate��5   	   $	��450��6    $	��800��6-   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ noteTextBox��5      $��0��6'    �� TextWrapping$
 Wrap=�64   $��	0,0,0,212q�6H   &   System.Windows.Controls.Calendar  5   
   -   5        $��datePicker1��5      $��Center=�6&      SelectionMode$ SingleRange=�6J   $��	0,202,0,0q�6f   �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   .+�5      �� -   5         ��Name$ 
saveButton��5      $��Сохранить��6)   $��0,0,10,0q�6=   6O   �� 5      -   5        $ cancelButton��5      $��Отмена��6+   $��0,0,10,0q�6<   6N   �� 5      -   5         ��Name$ addReminderCheckBox��5      $-��'Добавить напоминание��64   $
��Left=�6S   5      5      5      