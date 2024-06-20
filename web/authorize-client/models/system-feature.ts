export interface SystemFeature<TSettings> {
    id: string;
    name: string;
    type: string;
    settings: TSettings;
}

export interface SystemFeatureListItem {
    id: string;
    name: string;
    type: string;
}

export interface CreateSystemFeature {
    name: string;
    type: string;
}
