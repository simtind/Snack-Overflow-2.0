   M S B A M L   `   `   `  ����I  ESnackOverflowC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SnackOverflowCSnackOverflowC  5   	       SnackOverflowC.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:SnackOverflowC     5   	     � Title$  Snack Overflow��5   	   $	��720��6    $
��1080��6-   .��5	      � -   5         �Name$ 	grid_main��5	      .+�5
   
   � -   5        $ 	grid_idle��5
      .+�5      �� -   5         ��Name$
 logo��5      $
��0,30q�6"   $��logo.jpg��60   $��Center=�6B   $��Center=�6_   5      � 5   
   -   5        $ grid_purchase��5      $y�Hidden=�68   .+�5      �  � RowDefinitions 5      �� 5      $��98.429 �6$   �� 5      $��49.047 �6$   �� 5      $��49.047 �6$   �� 5      .+�5      �� -   5         ��Name$ p_tb_student��5      $
��Wrap=�62   $��Invalid Student��6F   $��64(�6]   � 5      $��1��6    � ColumnDefinitions 5      �� 5      $
��150* �6+   �� 5      $
��925* �6+   .+�5      �� -   5        $ 	textBlock��5       $
��Wrap=�63   $��32(�6G   $��Amount: ��6�   �� 5      -   5        $ p_tb_amount��5       $��1��65   $
��Wrap=�6E   $��0��6Y   $��32(�6b   5      � 5      $��2��6    5       �� 5!      $
��150* �6+   �� 5"      $
��925* �6+   .+�5$      �� -   5        $ 
textBlock1��5$       $
��Wrap=�64   $��32(�6H   $��	Balance: ��6�   �� 5%      -	   5        $ p_tb_balance��5%       $��1��66   $
��Wrap=�6F   $��0��6Z   $��32(�6c   5&      �� 5'      -
   5        $ p_image��5'      $��3��6)   5(      5)      � 5*   
   -   5        $ 	grid_cart��5*      $y�Hidden=�6"   .+�5+      � $
��Left=�6   $��30��6.   $��	10,0,0,60q�6:   $��Bottom=�6M   $��353.666��6h    5,      �� 5-      $
��101* �6'   �� 5.      $	��24* �6'   �� 5/      $
��229* �6'   .+�51      �� -   5        $ _resetin��51      $
��Wrap=�6.   $��Resetting in ��6B   $��18.667(�6�   �� 52      -   5        $ tb_resetting��52      $
��Wrap=�62   $��-0.002,0q�6F   $��1��6X    �� TextAlignment$ Right=�6h   $��18.667(�6�   �� 6�   $�25��6�   �� 6�   �� 6�   6   �� 53      -   5        $
 _sec��53      $
��Wrap=�6*   $��2��6>   �� 6�   $� ��6�   �� 6�   $��18.667(�6�   $�seconds��6�   6�   54      �� 55      -   5         ��Name$ sv_items��55      $��0,0,0,90q�6-   $
��Auto=�6?   |�.  .��56      �� -   5         ��Name$ sp_items��56      6/   57      � 58      $��60��6   $��Bottom=�6   $��10,0,0,0q�6:    59      �� 5:      $	��26* �6'   �� 5;      $
��315* �6'   �� 5<      $	��13* �6'   .+�5>      �� -   5        $ _total��5>      $
��Wrap=�6,   $��32(�6@   $��Total��6N   �� 5?      -   5        $ tb_total��5?      $��1��6.   $
��Wrap=�6>   $��0��6R   $��1,0,0,0q�6[   $��32(�6l   $ Right=�6�   �� 5@      -   5        $ _��5@      $��2��6'   $
��Wrap=�67   $��32(�6K   $��kr��6Y   $��1,0,10,14.833q�6c   5A      5B      5D      5E      