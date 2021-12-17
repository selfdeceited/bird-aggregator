import { ILifeListDto } from '../components/LifeList'
import { IYearlyLifeListDto } from './../components/YearlyLifeList'
import { fetchAs } from '.'

type LifeListResponse = { firstOccurences: ILifeListDto[] }
type YearlyLifeListResponse = { perYearCollection: IYearlyLifeListDto[] }

export async function fetchLifelist(): Promise<ILifeListDto[]> {
	const url = '/api/lifelist'
	const { firstOccurences } = await fetchAs<LifeListResponse>(url)
	return firstOccurences
}

export async function fetchYearlyLifelist(): Promise<IYearlyLifeListDto[]> {
	const url = '/api/lifelist'
	const { perYearCollection } = await fetchAs<YearlyLifeListResponse>(url)
	return perYearCollection
}

