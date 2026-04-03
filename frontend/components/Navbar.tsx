"use client";

import { useState, useEffect } from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';

import { authService } from '@/services/auth-service';

import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Button } from "@/components/ui/button"
import { User, LogOut, ShoppingCart, Package, Settings, LayoutDashboard } from "lucide-react"

export default function Navbar() {
  const [isScrolled, setIsScrolled] = useState(false);
  const [isMounted, setIsMounted] = useState(false);
  const pathname = usePathname();

  useEffect(() => {
    setIsMounted(true);
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 20);
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  const isLoggedIn = isMounted && authService.isLoggedIn();
  const user = isMounted ? authService.getUser() : null;

  if (pathname.startsWith('/admin')) {
    return null;
  }

  return (
    <header className={`header fixed top-0 w-full z-50 transition-all duration-300 ${isScrolled ? 'scrolled py-3' : 'py-5'}`}>
      <div className="container flex justify-between items-center">
        <Link href="/" className="text-2xl font-black tracking-tighter text-primary">FoodGrabber.</Link>
        
        <nav className="flex items-center gap-8">
          <Link href="/menu" className="text-sm font-semibold hover:text-primary/70 transition-colors">Menu</Link>
          <Link href="/about" className="text-sm font-semibold hover:text-primary/70 transition-colors">About</Link>
          
          {isLoggedIn ? (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="outline" className="rounded-full pl-2 pr-4 gap-2 h-10 border-border/50 hover:bg-secondary/50 transition-all">
                  <div className="w-7 h-7 rounded-full bg-primary text-primary-foreground flex items-center justify-center text-[10px] font-bold">
                    {user?.email[0].toUpperCase()}
                  </div>
                  <span className="text-sm font-bold">{user?.email.split('@')[0]}</span>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end" className="w-56 mt-2 rounded-2xl p-2 shadow-2xl border-border/40 backdrop-blur-xl bg-white/95">
                <DropdownMenuLabel className="px-3 py-3">
                  <div className="flex flex-col gap-1">
                    <p className="text-[10px] font-bold uppercase tracking-widest text-muted-foreground">Account</p>
                    <p className="text-sm font-bold truncate">{user?.email}</p>
                  </div>
                </DropdownMenuLabel>
                <DropdownMenuSeparator className="bg-border/40" />
                
                <div className="p-1">
                  {authService.isAdmin() && (
                    <DropdownMenuItem asChild className="focus:bg-primary focus:text-white">
                      <Link href="/admin/dashboard" className="group flex items-center gap-3 px-3 py-2.5 rounded-xl cursor-pointer transition-all">
                        <LayoutDashboard className="w-4 h-4 opacity-70 group-hover:opacity-100 transition-opacity" />
                        <span className="font-medium">Admin Dashboard</span>
                      </Link>
                    </DropdownMenuItem>
                  )}
                  <DropdownMenuItem asChild className="focus:bg-primary focus:text-white">
                    <Link href="/orders" className="group flex items-center gap-3 px-3 py-2.5 rounded-xl cursor-pointer transition-all">
                      <Package className="w-4 h-4 opacity-70 group-hover:opacity-100 transition-opacity" />
                      <span className="font-medium">My Orders</span>
                    </Link>
                  </DropdownMenuItem>
                  <DropdownMenuItem asChild className="focus:bg-primary focus:text-white">
                    <Link href="/cart" className="group flex items-center gap-3 px-3 py-2.5 rounded-xl cursor-pointer transition-all">
                      <ShoppingCart className="w-4 h-4 opacity-70 group-hover:opacity-100 transition-opacity" />
                      <span className="font-medium">My Cart</span>
                    </Link>
                  </DropdownMenuItem>
                  <DropdownMenuItem asChild className="focus:bg-primary focus:text-white">
                    <Link href="/profile" className="group flex items-center gap-3 px-3 py-2.5 rounded-xl cursor-pointer transition-all">
                      <User className="w-4 h-4 opacity-70 group-hover:opacity-100 transition-opacity" />
                      <span className="font-medium">Profile Settings</span>
                    </Link>
                  </DropdownMenuItem>
                </div>

                <DropdownMenuSeparator className="bg-border/40" />
                <div className="p-1">
                  <DropdownMenuItem 
                    className="flex items-center gap-3 px-3 py-2.5 rounded-xl cursor-pointer text-destructive focus:text-destructive hover:bg-destructive/5 transition-all"
                    onClick={() => authService.logout()}
                  >
                    <LogOut className="w-4 h-4" />
                    <span className="font-bold">Sign Out</span>
                  </DropdownMenuItem>
                </div>
              </DropdownMenuContent>
            </DropdownMenu>
          ) : (
            <Button asChild variant="default" className="rounded-full px-6 font-bold shadow-lg shadow-primary/10 transition-all hover:scale-[1.02] active:scale-[0.98]">
              <Link href="/login">Sign In</Link>
            </Button>
          )}
        </nav>
      </div>
    </header>
  );
}
