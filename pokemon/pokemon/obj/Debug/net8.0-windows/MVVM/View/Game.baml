   M S B A M L   `   `   `  ����B  >pokemon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:pokemon.MVVM.Viewpokemon.MVVM.View  5      ?$clr-namespace:pokemon.MVVM.ViewModelpokemon.MVVM.ViewModel  5          pokemon.MVVM.View.Game   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:pokemon.MVVM.View     5      3VM$clr-namespace:pokemon.MVVM.ViewModel     5        � 	Resources  5       �� 
TargetType �� Property �� Value��%�  '��       '�� �     �� 5   
   " ��6   .�5      ��  � ��$ 16(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 0,0,0,10q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$ 5q�6(   �� 5       � ��$ 	0,10,0,10q�6'   �� 5       � ��$	 100��6*   �� 5       � ��
 �����6,   �� 5       � ��$ 1q�60   �� 5       � ��
 �����6+   �� 5       � ��$
 Auto=�6I   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5       $��* �6   �� 5!      $��* �6   .+�5$   
   �� $��0��6   $��10q�6%   .+�5%      �� $��Available Pokémon��6   �� 5'       �� DisplayMemberPath$
 Name��5(       �� SelectionMode$ Single=�5*      ��5'      �� *
Pokemons+ �� SelectedItem 5)      �� *SelectedPlayerPokemon+5*      �� 5,      $��Player Selection��6   �� 5.      ��6   �� *PlayerPokemonList+��5/      �� 50      .R�51      �� ��6$   �� *Name+52      54      55      �� 57   
   $��1��6   $��10q�6%   .+�58      �� $��Available Pokémon��6   �� 5:      $
 Name��5;      $ Single=�5=      ��5:      �� *
Pokemons+ 5<      �� *SelectedEnemyPokemon+5=      �� 5?      $��Enemy Selection��6   �� 5A      ��6   �� *EnemyPokemonList+��5B      �� 5C      .R�5D      �� ��6$   �� *Name+5E      5G      5H      �� 5J   
   $��0��6   $��2��6%   $��Center=�69   $��Bottom=�6V   $��0,0,0,10q�6q   .+�5K      �� $��Validate Selections��6   ��5L      �� *ValidateSelectionsCommand+5M      5N      5P      