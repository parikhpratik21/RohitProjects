   M S B A M L   `   `   `  ����F  BCentral LED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Central_LED.ControlsCentral_LED.Controls  5   	       Central_LED.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6control"clr-namespace:Central_LED.Controls     5   	   -   5          � Title$  Central LED��5   	   $	��550��6   $	��600��6*    � 	Resources 5         FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray  Item.MouseOver.Background �� Color  Item.MouseOver.Border $  Item.SelectedInactive.Background   Item.SelectedInactive.Border " Item.SelectedActive.Background  Item.SelectedActive.Border  ListBoxSelectedItemStyle �� 
TargetType �� RelativeSource	 �� AncestorType
 �� 
TargetType ��Name �� Padding �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName P� Property P� Value��%i  &	        &	 �     &	 �     &	 �     &	     &	 6    &	 Y    &	 |    7   7  7  7  7  7  �� 5	   
   .�5
      ��  � �� 5      �� 5      .R�5      �� $��2q�6$    . #��� ��6J   $��1��6�   $	 1 2R�6�   5      5      5      �� 5   
   $ 	#1F26A0DA��6<   �� 5   
   $ 	#a826A0Da��68   �� 5   
   $ 	#3DDADADA��6C   �� 5   
   $ 	#FFDADADA��6?   �� 5   
   $ 	#3D26A0DA��6A   �� 5   
   $ 	#FF26A0DA��6=   �� 5   
   " ��61   .�5      ��  �@J�SnapsToDevicePixels . 64   �� 5       � ��$	 4,1q�6(   �� 5       � �� 6;   �� *HorizontalContentAlignment+ �� "	 ���� 5       � �� 69   �� *VerticalContentAlignment+ �� "	 ���� 5       � ��
 ���� 6+   �� 5       � ��
 ���� 6,   �� 5       � ��$ 1q�60   �� 5        �@�FocusVisualStyle8   61   �� 5!       � �� 5"      �� 5#      "
 ��6&   .R�5$      �� $ Bd��6!   #��z��6-   #��z��6Y   #��z��6�   # z��6�    . ._�5%      �� #��z��6/   # z 6r   #��z��6�   5&       5'      ]� 5(       5)   "   �� 5*   &    �@J�IsMouseOver . 6G   ./�5,   "   �� $ Bd��6?   8   � ��6O   �� 5-   "   $ Bd��6@   8   � ��6P   5.      ]� 5/       50   "   �� 51   &    �@��IsSelectionActive .  6V   �� 52   &    �@��
IsSelected . 6F   ./�54   "   �� $ Bd��6?    � ��
 ��ح�6O   �� 55   "   $ Bd��6@   8   � ��6P   56      ]� 57       58   "   �� 59   &    �@��IsSelectionActive . 6V   �� 5:   &    �@��
IsSelected . 6F   ./�5<   "   �� $ Bd��6?   8   � ��6O   �� 5=   "   $ Bd��6@   8   � ��6P   5>      P� 5?       � {� .  . �5@   "   �� $ Bd��6K   # � ��6[    � ��5A      5C      5E      5F      5G      .��5H      �  � RowDefinitions 5I   
   �� 5J      $
��Auto �6   �� 5K      $��* �6   .+�5M   
   o� $��0��6   .@�5N      m� $��Setting��6   .@�5O      m� -   5         m�Name$ miModbus��5O   3   $��Modbus Setting��6   6E   m� 5P      -   5        $ 	miDisplay��5P   4   $��Display Setting��6   6G   5Q      m� 5R      -   5        $ 	MiAboutUs��5R   )   $��About us��6   6<   5S      � 5T   
   $��1��6    � ColumnDefinitions 5U      �� 5V      $	��150 �6#   �� 5W      $��5* �6#   .+�5Z      �� -   5         ��Name$ dataList��5Z      $��0��6(   $��
10 10 0 10q�68   $��0��6L    �� DisplayMemberPath$ DisplayName��6�   #���  6�   ��6Y   �� *DisplayControlList+6�   �� 5[      $��1��6   $
��Auto=�6+   .��5\      0   *Central_LED.Controls.LiveDisplayController  -   5          Name$ LiveDisplay��5\   0   $��10q�6E   $��Stretch=�6Q   $��Stretch=�6o   5]      5^      5_      5`      