import React from 'react';
import { PublicHeader } from './PublicHeader';
import { PublicFooter } from './PublicFooter';
// import { PublicHeader, PublicFooter } from ''


export interface PublicLayoutProps {
  children: React.ReactNode;
}

export const PublicLayout = ({ children }: PublicLayoutProps) => {
  return (
    <>
      <PublicHeader />
      {/* <main className="flex-1">{children}</main> */}
      <main className="flex-1 flex items-center justify-center bg-gray-50">
        {children}
      </main>
      <PublicFooter />
    </>
  );
};
