   M S B A M L   `   `   `  ����  BarcodeScanner    BarcodeScanner.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5	      $��* �6   �� 5
      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��10q�6;   .+�5      �� $��Select excel file:��6   $��Center=�62   �� 5      -   5         ��Name$ 	btnBrowse��5   8   $��Browse��6   $��10 0 0 0q�6&   6K   �� 5      -   5         ��Name$ txtFilePath��5      $��10 0 0 0q�6-   $��Center=�6?   5      �� 5   
   -   5        $ btnRead��5      $��1��5      $��Right=�6   $	��Top=�6:   $
��Read��6R   $��50��6a   $��10q�6l   5   "   �� 5   
   $��1��6   $ 
Horizontal=�6"   $��Center=�6;   $��Bottom=�6X   $��10q�6s   .+�5      �� $��Total scanned barcode: ��6   �� 5      -   5        $ txtTotalBarcode��5      $��10 0 0 0q�61   5      � 5   
   $��2��6   .+�5      �� -   5         ��Name$ txtBarcodeValue��5      6/   5      �� 5   
   -   5        $ txtBarcodeScaneMessage��5      $��3��64   $��Center=�6A   $��Center=�6\   5      5      