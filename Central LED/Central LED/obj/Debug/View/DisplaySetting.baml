   M S B A M L   `   `   `  ����F  BCentral LED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Central_LED.ControlsCentral_LED.Controls  5   	        Central_LED.DisplaySetting   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6control"clr-namespace:Central_LED.Controls     5   	   -   5          � Title$  Display Setting��5   	   $	��400��6!   $	��550��6.    � 	Resources 5         FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray  Item.MouseOver.Background �� Color  Item.MouseOver.Border $  Item.SelectedInactive.Background   Item.SelectedInactive.Border " Item.SelectedActive.Background  Item.SelectedActive.Border  ListBoxSelectedItemStyle �� 
TargetType �� RelativeSource	 �� AncestorType
 �� 
TargetType ��Name �� Padding �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName P� Property P� Value��%i  &	        &	 �     &	 �     &	 �     &	     &	 6    &	 Y    &	 |    7   7  7  7  7  7  �� 5   
   .�5	      ��  � �� 5
      �� 5      .R�5      �� $��2q�6$    . #��� ��6J   $��1��6�   $	 1 2R�6�   5      5      5      �� 5   
   $ 	#1F26A0DA��6<   �� 5   
   $ 	#a826A0Da��68   �� 5   
   $ 	#3DDADADA��6C   �� 5   
   $ 	#FFDADADA��6?   �� 5   
   $ 	#3D26A0DA��6A   �� 5   
   $ 	#FF26A0DA��6=   �� 5   
   " ��61   .�5      ��  �@J�SnapsToDevicePixels . 64   �� 5       � ��$	 4,1q�6(   �� 5       � �� 6;   �� *HorizontalContentAlignment+ �� "	 ���� 5       � �� 69   �� *VerticalContentAlignment+ �� "	 ���� 5       � ��
 ���� 6+   �� 5       � ��
 ���� 6,   �� 5       � ��$ 1q�60   �� 5       �@�FocusVisualStyle8   61   �� 5        � �� 5!      �� 5"      "
 ��6&   .R�5#      �� $ Bd��6!   #��z��6-   #��z��6Y   #��z��6�   # z��6�    . ._�5$      �� #��z��6/   # z 6r   #��z��6�   5%       5&      ]� 5'       5(   "   �� 5)   &    �@J�IsMouseOver . 6G   ./�5+   "   �� $ Bd��6?   8   � ��6O   �� 5,   "   $ Bd��6@   8   � ��6P   5-      ]� 5.       5/   "   �� 50   &    �@��IsSelectionActive .  6V   �� 51   &    �@��
IsSelected . 6F   ./�53   "   �� $ Bd��6?    � ��
 ��ح�6O   �� 54   "   $ Bd��6@   8   � ��6P   55      ]� 56       57   "   �� 58   &    �@��IsSelectionActive . 6V   �� 59   &    �@��
IsSelected . 6F   ./�5;   "   �� $ Bd��6?   8   � ��6O   �� 5<   "   $ Bd��6@   8   � ��6P   5=      P� 5>       � {� .  . �5?   "   �� $ Bd��6K   # � ��6[    � ��5@      5B      5D      5E      5F      .��5G      �  � RowDefinitions 5H   
   �� 5I      $��* �6   �� 5J      $
��Auto �6    � ColumnDefinitions 5L   
   �� 5M      $
��1.5* �6   �� 5N      $��5* �6   .+�5Q   
   �� $��0��6   $��
10 10 0 10q�6"   $��0��66    �� DisplayMemberPath$ DisplayName��6n   #���  6�   ��6C   �� *DisplayControlList+ �� SelectedItem 6�   �� *SelectedDisplayControl+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   )   #Central_LED.Controls.DisplayControl  5R   
   $��0��6!   $��1��6]   $��10q�6m    � DataContext 6.   �� *SelectedDisplayControl+6m   � 5T   
   $��1��6   $��0��6   $��	10 0 0 10q�6,   $��2��6?    5U      �� 5V      $
��0.5* �6#   �� 5W      $��* �6#   �� 5X      $��* �6#   �� 5Y      $��4* �6#   .+�5[      �� $��Display ��6   $��0��6(   �� 5\      -   5         ��Name$ btnAddDisplay��5\   Z   $��1��6   $	��Add��6%   $��60��63   $��Right=�6>   6q   �� 5]      -   5        $ btnRemoveDisplay��5]   S   $��2��6   $��Remove��6%   $��60��66   $��10 0 0 0q�6A   6m   �� 5^      -   5        $ btnSaveDisplay��5^   n   $��3��6   $
��Send��6%   $��60��64   $��	10 0 10 0q�6?   $��Right=�6R   6�   5_      5a      5b      