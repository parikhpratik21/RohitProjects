   M S B A M L   `   `   `  ����F  BCentral LED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Central_LED.ControlsCentral_LED.Controls  5      )    #Central_LED.Controls.DisplayControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7controls"clr-namespace:Central_LED.Controls     5      -   5          � 	Resources  5         FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray  Item.MouseOver.Background �� Color  Item.MouseOver.Border $  Item.SelectedInactive.Background   Item.SelectedInactive.Border " Item.SelectedActive.Background  Item.SelectedActive.Border  ListBoxSelectedItemStyle �� 
TargetType �� RelativeSource �� AncestorType	 �� 
TargetType
 ��Name �� Padding �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName P� Property P� Value��%i  &	        &	 �     &	 �     &	 �     &	     &	 6    &	 Y    &	 |    7   7  7  7  7  7  �� 5   
   .�5      ��  � �� 5      �� 5      .R�5      �� $��2q�6$    . #��� ��6J   $��1��6�   $	 1 2R�6�   5      5      5      �� 5   
   $ 	#1F26A0DA��6<   �� 5   
   $ 	#a826A0Da��68   �� 5   
   $ 	#3DDADADA��6C   �� 5   
   $ 	#FFDADADA��6?   �� 5   
   $ 	#3D26A0DA��6A   �� 5   
   $ 	#FF26A0DA��6=   �� 5   
   " ��61   .�5      ��  �@J�SnapsToDevicePixels . 64   �� 5       � ��$	 4,1q�6(   �� 5       � �� 6;   �� *HorizontalContentAlignment+ �� " ���� 5       � �� 69   �� *VerticalContentAlignment+ �� " ���� 5        � ��
 ���� 6+   �� 5!       � ��
 ���� 6,   �� 5"       � ��$ 1q�60   �� 5#       �@�FocusVisualStyle8   61   �� 5$       � �� 5%      �� 5&      "	 ��6&   .R�5'      �� $
 Bd��6!   #��z��6-   #��z��6Y   #��z��6�   # z��6�    . ._�5(      �� #��z��6/   # z 6r   #��z��6�   5)       5*      ]� 5+       5,   "   �� 5-   &    �@J�IsMouseOver . 6G   ./�5/   "   �� $ Bd��6?   8   � ��6O   �� 50   "   $ Bd��6@   8   � ��6P   51      ]� 52       53   "   �� 54   &    �@��IsSelectionActive .  6V   �� 55   &    �@��
IsSelected . 6F   ./�57   "   �� $ Bd��6?    � ��
 ��ح�6O   �� 58   "   $ Bd��6@   8   � ��6P   59      ]� 5:       5;   "   �� 5<   &    �@��IsSelectionActive . 6V   �� 5=   &    �@��
IsSelected . 6F   ./�5?   "   �� $ Bd��6?   8   � ��6O   �� 5@   "   $ Bd��6@   8   � ��6P   5A      P� 5B       � {� .  . �5C   "   �� $ Bd��6K   # � ��6[    � ��5D      5F      5H      5I      5J      .��5K      � -   5         �Name$ MainGrid��5K       � RowDefinitions 5L   
   �� 5M      $
��Auto �6   �� 5N      $
��Auto �6   �� 5O      $
��Auto �6   �� 5P      $��* �6    � ColumnDefinitions 5R   
   �� 5S      $��* �6   �� 5T      $��* �6   �� 5U      $
��1.1* �6   �� 5V      $��* �6   .+�5Y   
   �� $��0��6   $��0��6!   $��Display Name��61   $��0 0 10 5q�6E   $��Right=�6W   $��Center=�6s   �� 5Z   
   $��0��6   $��1��6   $��3��60   $��0 0 10 5q�6D   $��Stretch=�6s   $��Center=�6�   ��6V   �� *DisplayName+6�   �� 5\   
   $��1��6   $��0��6!   $��IP��61   $��0 0 10 5q�6;   $��Right=�6M   $��Center=�6i   �� 5]   
   $��1��6   $��1��6   $��0 0 10 5q�60   $��Stretch=�6V   $��Center=�6t   ��6B   �� *IP+6t   �� 5_   
   $��1��6   $��2��6!   $��0 0 10 5q�61   $
��Port��6C   $��Right=�6O   $��Center=�6k   �� 5`   
   $��1��6   $��3��6   $��0 0 10 5q�6E   $��Stretch=�6W   $��Center=�6u   ��6/   �� *Port+6u   �� 5b   
   $��2��6   $��0��6!   $��0 0 10 5q�61   $��Lines��6C   $��Right=�6P   $��Center=�6l   �� 5c   
   $��2��6   $��1��6   $��0 0 10 5q�6F   $��Stretch=�6X   $��Center=�6v   ��6/   �� *Lines+6v   �� 5e   
   $��2��6   $��2��6!   $��0 0 10 5q�61   $��Data Column��6C   $��Right=�6V   $��Center=�6r   �� 5f   
   $��2��6    �� 	MaxLength$ 1��6   $��3��6-   $��0 0 10 5q�6Z   $��Stretch=�6l   $��Center=�6�   ��6=   �� *DataColumns+6�   �� 5h   
   $��0��6   $��3��6"   $��0 10 0 5q�6/    �� DisplayMemberPath$ LineName��6b   #���  6�   ��6A   �� *
LineList+ �� SelectedItem 6   �� *SelectedLine+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   -   'Central_LED.Controls.LineSettingControl  5i   
   -   5          Name$ lineSettingControl��5i   &   $��10 0 0 5q�6B   $��3��6T   $��1��6�   $��3��6�    � DataContext 6a   �� *SelectedLine+6�   5k      5l      