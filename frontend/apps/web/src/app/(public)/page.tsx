"use client";

import Link from "next/link";
import { Button } from "@packages/ui";
import {
  ShieldCheck,
  BarChart3,
  Layers,
  Lock,
  Globe,
  FileCheck
} from "lucide-react";

export default function LandingPage() {
  return (
    <main className="min-h-screen bg-gray-50 text-gray-900 flex flex-col">
      {/* Hero */}
      <section className="px-4 md:px-20 py-14 bg-white text-center">
        <h2 className="text-2xl md:text-4xl font-semibold mb-4">
          Secure Financial Management Platform
        </h2>

        <p className="text-gray-600 max-w-2xl mx-auto mb-8 text-sm md:text-lg">
          A reliable platform to manage transactions, monitor performance,
          and access financial insights with complete transparency.
        </p>

        <Link href="/register">
          <Button size="lg">Get Started</Button>
        </Link>
      </section>

      {/* Services */}
      <section className="px-4 md:px-20 py-14">
        <h3 className="text-xl md:text-2xl font-semibold text-center mb-10">
          Core Services
        </h3>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <ServiceCard
            icon={<ShieldCheck className="text-blue-600" />}
            title="Secure Operations"
            desc="End-to-end encrypted data handling following financial security standards."
          />
          <ServiceCard
            icon={<BarChart3 className="text-blue-600" />}
            title="Financial Insights"
            desc="Clear summaries and reports for better financial decisions."
          />
          <ServiceCard
            icon={<Layers className="text-blue-600" />}
            title="Unified Dashboard"
            desc="All accounts, transactions, and reports in one place."
          />
        </div>
      </section>

      {/* Trust & Compliance */}
      <section className="px-4 md:px-20 py-14 bg-white border-t">
        <h3 className="text-xl md:text-2xl font-semibold text-center mb-8">
          Security & Compliance
        </h3>

        <div className="grid grid-cols-2 md:grid-cols-4 gap-6 text-center text-sm">
          <TrustBadge icon={<Lock />} label="SSL Secured" />
          <TrustBadge icon={<FileCheck />} label="ISO Certified" />
          <TrustBadge icon={<Globe />} label="GDPR Compliant" />
          <TrustBadge icon={<ShieldCheck />} label="RBI Guidelines" />
        </div>
      </section>
    </main>
  );
}

/* Components */
function ServiceCard({ icon, title, desc }: any) {
  return (
    <div className="bg-white p-6 rounded-md border">
      <div className="mb-3">{icon}</div>
      <h4 className="font-medium mb-2">{title}</h4>
      <p className="text-sm text-gray-600">{desc}</p>
    </div>
  );
}

function TrustBadge({ icon, label }: any) {
  return (
    <div className="flex flex-col items-center gap-2">
      <div className="p-3 bg-gray-100 rounded-full text-blue-600">
        {icon}
      </div>
      <span className="text-gray-700">{label}</span>
    </div>
  );
}
